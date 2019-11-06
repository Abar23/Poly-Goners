using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour
{

    /*Reference both the transforms of the two players on screen.
	Necessary to find out their current positions.*/
    private Transform player1;
    private Transform player2;

    //The distance at which the splitscreen will be activated.
    public float splitDistance = 5;

    //The color and width of the splitter which splits the two screens up.
    public Color splitterColor;
    public float splitterWidth;

    // Transfrom positions for the cameras for simple changes and testing
    public float cameraX = 0f;
    public float cameraY = 14f;
    public float cameraZ = -5f;
    public float PlayerTopScreenOffset = -7.5f;
    public float PlayerBottomScreenOffset = 3.0f;
    public float PlayerLeftScreenOffset = 7.0f;
    public float PlayerRightScreenOffset = -7.0f;
    public float yOffsetWhenPlayersAreSeparated = 3.0f;
    public float secondarySplitDistance = 5.0f;

    private LayerMask raycastLayerMask;

    //The two cameras, both of which are initalized/referenced in the start function.
    private GameObject camera1;
    private GameObject camera2;

    //The two quads used to draw the second screen, both of which are initalized in the start function.
    private GameObject split;
    private GameObject splitter;

    private Vector3 previousMidpoint;
    private Vector3 previousMidpoint2;

    private HashSet<GameObject> setOfWalls; 

    void Start()
    {
        this.raycastLayerMask = LayerMask.GetMask("CameraBlocker");
        this.setOfWalls = new HashSet<GameObject>();
        //Referencing camera1 and initalizing camera2.
        camera1 = Camera.main.gameObject;
        camera2 = new GameObject();
        camera2.AddComponent<Camera>();
        //Setting up the culling mask of camera2 to ignore the layer "TransparentFX" as to avoid rendering the split and splitter on both cameras.
        camera2.GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("TransparentFX"));
        camera2.AddComponent<CameraShake>();
        camera2.tag = "MainCamera";

        //Setting up the splitter and initalizing the gameobject.
        splitter = GameObject.CreatePrimitive(PrimitiveType.Quad);
        splitter.transform.parent = gameObject.transform;
        splitter.transform.localPosition = Vector3.forward;
        splitter.transform.localScale = new Vector3(3, splitterWidth / 10, 1);
        splitter.transform.localEulerAngles = Vector3.zero;
        splitter.SetActive(false);

        //Setting up the split and initalizing the gameobject.
        split = GameObject.CreatePrimitive(PrimitiveType.Quad);
        split.transform.parent = splitter.transform;
        split.transform.localPosition = new Vector3(0, -(1 / (splitterWidth / 10)), 0);
        split.transform.localScale = new Vector3(1, 2 / (splitterWidth / 10), 1);
        split.transform.localEulerAngles = Vector3.zero;

        //Creates both temporary materials required to create the splitscreen.
        Material tempMat = new Material(Shader.Find("Unlit/Color"));
        tempMat.color = splitterColor;
        splitter.GetComponent<Renderer>().material = tempMat;
        splitter.GetComponent<Renderer>().sortingOrder = 2;
        splitter.layer = LayerMask.NameToLayer("TransparentFX");
        Material tempMat2 = new Material(Shader.Find("Mask/SplitScreen"));
        split.GetComponent<Renderer>().material = tempMat2;
        split.layer = LayerMask.NameToLayer("TransparentFX");
    }

    void LateUpdate()
    {
        GameObject player1GameObject = PlayerManager.GetInstance().GetPlayerOneGameObject();
        GameObject player2GameObject = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        if (player1GameObject != null && player2GameObject != null)
        {
            this.player1 = player1GameObject.transform;
            this.player2 = player2GameObject.transform;

            if (player1.gameObject.activeSelf && player2.gameObject.activeSelf)
            {
                //Gets the z axis distance between the two players and just the standard distance.
                float zDistance = player1.position.z - player2.transform.position.z;
                float distance = Vector3.Distance(player1.position, player2.transform.position);

                //Sets the angle of the player up, depending on who's leading on the x axis.
                float angle;
                if (player1.transform.position.x <= player2.transform.position.x)
                {
                    angle = Mathf.Rad2Deg * Mathf.Acos(zDistance / distance);
                }
                else
                {
                    angle = Mathf.Rad2Deg * Mathf.Asin(zDistance / distance) - 90;
                }

                //Rotates the splitter according to the new angle.
                splitter.transform.localEulerAngles = new Vector3(0, 0, angle);

                //Gets the exact midpoint between the two players.
                Vector3 midPoint = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, (player1.position.z + player2.position.z) / 2);
                Vector3 midPoint2 = midPoint;

                float cameraYOffset = mapRange(distance, 0, this.splitDistance, 11.0f, this.splitDistance + this.secondarySplitDistance);

                //Waits for the two cameras to split and then calcuates a midpoint relevant to the difference in position between the two cameras.
                if (distance > splitDistance)
                {
                    Vector3 offset = midPoint - player1.position;
                    offset.x = Mathf.Clamp(offset.x, -splitDistance / 2, splitDistance / 2);
                    offset.y = Mathf.Clamp(offset.y, -splitDistance / 2, splitDistance / 2);
                    offset.z = Mathf.Clamp(offset.z, -splitDistance / 2, splitDistance / 2);

                    Vector3 offset2 = midPoint - player2.position;
                    offset2.x = Mathf.Clamp(offset.x, -splitDistance / 2, splitDistance / 2);
                    offset2.y = Mathf.Clamp(offset.y, -splitDistance / 2, splitDistance / 2);
                    offset2.z = Mathf.Clamp(offset.z, -splitDistance / 2, splitDistance / 2);

                    midPoint = player1.position + offset;
                    midPoint2 = player2.position - offset2;

                    //Sets the splitter and camera to active and sets the second camera position as to avoid lerping continuity errors.
                    if (splitter.activeSelf == false)
                    {
                        splitter.SetActive(true);
                        camera2.SetActive(true);

                        camera2.transform.position = camera1.transform.position;
                        camera2.transform.rotation = camera1.transform.rotation;
                    }
                    if(distance > this.splitDistance + this.secondarySplitDistance)
                    {
                        midPoint = Vector3.Lerp(this.previousMidpoint, player1.position + offset.normalized * 3.5f, Time.deltaTime * 5.0f);
                        midPoint2 = Vector3.Lerp(this.previousMidpoint2, player2.position - offset2.normalized * 3.5f, Time.deltaTime * 5.0f);

                        if (Mathf.Abs(angle) > 0.0f && Mathf.Abs(angle) <= 35.0f || Mathf.Abs(angle) > 145.0f && Mathf.Abs(angle) <= 180.0f)
                        {
                            float playerTwoOffset = (player2.transform.position.z >= player1.transform.position.z) ? PlayerTopScreenOffset : PlayerBottomScreenOffset;
                            float playerOneOffset = (playerTwoOffset > 0.0f) ? PlayerTopScreenOffset : PlayerBottomScreenOffset;

                            camera2.transform.position = Vector3.Lerp(camera2.transform.position, player2.transform.position + new Vector3(cameraX, cameraY + yOffsetWhenPlayersAreSeparated, cameraZ + playerTwoOffset), Time.deltaTime * 3.5f);
                            camera1.transform.position = Vector3.Lerp(camera1.transform.position, player1.transform.position + new Vector3(cameraX, cameraY + yOffsetWhenPlayersAreSeparated, cameraZ + playerOneOffset), Time.deltaTime * 3.5f);
                        }
                        else
                        {
                            float playerTwoOffset = (player2.transform.position.x >= player1.transform.position.x) ? PlayerRightScreenOffset : PlayerLeftScreenOffset;
                            float playerOneOffset = (playerTwoOffset > 0.0f) ? PlayerRightScreenOffset : PlayerLeftScreenOffset;

                            camera2.transform.position = Vector3.Lerp(camera2.transform.position, player2.transform.position + new Vector3(cameraX + playerTwoOffset, cameraY + yOffsetWhenPlayersAreSeparated, cameraZ), Time.deltaTime * 3.5f);
                            camera1.transform.position = Vector3.Lerp(camera1.transform.position, player1.transform.position + new Vector3(cameraX + playerOneOffset, cameraY + yOffsetWhenPlayersAreSeparated, cameraZ), Time.deltaTime * 3.5f);
                        }
                    }
                    else
                    {
                        //Lerps the second cameras position and rotation to that of the second midpoint, so relative to the second player.
                        camera2.transform.position = Vector3.Lerp(camera2.transform.position, midPoint2 + new Vector3(cameraX, cameraYOffset, cameraZ), Time.deltaTime * 3.5f);
                        Quaternion newRot2 = Quaternion.LookRotation(midPoint2 - camera2.transform.position);
                        newRot2.eulerAngles = new Vector3(newRot2.eulerAngles.x, 0.0f, newRot2.eulerAngles.z);
                        camera2.transform.rotation = Quaternion.Lerp(camera2.transform.rotation, newRot2, Time.deltaTime * 3.5f);
                    }
                }
                else
                {
                    //Deactivates the splitter and camera once the distance is less than the splitting distance (assuming it was at one point).
                    if (splitter.activeSelf)
                        splitter.SetActive(false);
                    camera2.SetActive(false);
                }

                if(distance < this.splitDistance + this.secondarySplitDistance)
                {
                    /*Lerps the first cameras position and rotation to that of the second midpoint, so relative to the first player
                    or when both players are in view it lerps the camera to their midpoint.*/
                    camera1.transform.position = Vector3.Lerp(camera1.transform.position, midPoint + new Vector3(cameraX, cameraYOffset, cameraZ), Time.deltaTime * 3.5f);
                    Quaternion newRot = Quaternion.LookRotation(midPoint - camera1.transform.position);
                    newRot.eulerAngles = new Vector3(newRot.eulerAngles.x, 0.0f, newRot.eulerAngles.z);
                    camera1.transform.rotation = Quaternion.Lerp(camera1.transform.rotation, newRot, Time.deltaTime * 3.5f);
                }

                this.previousMidpoint = midPoint;
                this.previousMidpoint2 = midPoint2;
            }
            else if (player1.gameObject.activeSelf && !player2.gameObject.activeSelf)
            {
                if (splitter.activeSelf)
                    splitter.SetActive(false);
                camera2.SetActive(false);

                /*Lerps the first cameras position and rotation to that first players posisiton since it is the only active player on the screen.*/
                camera1.transform.position = Vector3.Lerp(camera1.transform.position, player1.position + new Vector3(cameraX, cameraY, cameraZ), Time.deltaTime * 3.5f);
                Quaternion newRot = Quaternion.LookRotation(player1.position - camera1.transform.position);
                newRot.eulerAngles = new Vector3(newRot.eulerAngles.x, 0.0f, newRot.eulerAngles.z);
                camera1.transform.rotation = Quaternion.Lerp(camera1.transform.rotation, newRot, 1.0f);
            }
            else if (player2.gameObject.activeSelf && !player1.gameObject.activeSelf)
            {
                if (splitter.activeSelf)
                    splitter.SetActive(false);
                camera2.SetActive(false);

                /*Lerps the first cameras position and rotation to that first players posisiton since it is the only active player on the screen.*/
                camera1.transform.position = Vector3.Lerp(camera1.transform.position, player2.position + new Vector3(cameraX, cameraY, cameraZ), Time.deltaTime * 3.5f);
                Quaternion newRot = Quaternion.LookRotation(player2.position - camera1.transform.position);
                newRot.eulerAngles = new Vector3(newRot.eulerAngles.x, 0.0f, newRot.eulerAngles.z);
                camera1.transform.rotation = Quaternion.Lerp(camera1.transform.rotation, newRot, 1.0f);
            }
        }
    }

    void FixedUpdate()
    {
        GameObject player1GameObject = PlayerManager.GetInstance().GetPlayerOneGameObject();
        GameObject player2GameObject = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        HashSet<GameObject> raycastedWalls = null;

        if (player1GameObject != null && player2GameObject != null)
        {
            this.player1 = player1GameObject.transform;
            this.player2 = player2GameObject.transform;

            if (player1GameObject.gameObject.activeSelf && player2GameObject.gameObject.activeSelf)
            {
                //Gets the z axis distance between the two players and just the standard distance.
                float zDistance = player1.position.z - player2.transform.position.z;
                float distance = Vector3.Distance(player1.position, player2.transform.position);

                if (distance > splitDistance)
                {
                    raycastedWalls = SetWallsTransparent(FindObjectBetweenCamperaAndPlayer(this.player1, this.camera1),
                        FindObjectBetweenCamperaAndPlayer(this.player2, this.camera2)
                    );
                }
                else
                {
                    raycastedWalls = SetWallsTransparent(FindObjectBetweenCamperaAndPlayer(this.player1, this.camera1),
                        FindObjectBetweenCamperaAndPlayer(this.player2, this.camera1)
                    );
                }
            }
            else if (player1GameObject.gameObject.activeSelf && !player2GameObject.gameObject.activeSelf)
            {
                raycastedWalls = SetWallsTransparent(FindObjectBetweenCamperaAndPlayer(this.player1, this.camera1),
                    null
                );
            }
            else if (player2GameObject.gameObject.activeSelf && !player1GameObject.gameObject.activeSelf)
            {
                raycastedWalls = SetWallsTransparent(FindObjectBetweenCamperaAndPlayer(this.player2, this.camera1),
                    null
                );
            }
        }

        RemoveWallsNotBetweenCameraAndPlayers(raycastedWalls);
    }

    private RaycastHit[] FindObjectBetweenCamperaAndPlayer(Transform player, GameObject camera)
    {
        float dist = Vector3.Distance(camera.transform.position, player.transform.position);
        Vector3 raycastDirection = (player.transform.position - camera.transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(camera.transform.position, raycastDirection, dist, this.raycastLayerMask);
        return hits;
    }

    HashSet<GameObject> SetWallsTransparent(RaycastHit[] playerOneHits, RaycastHit[] playerTwoHits)
    {
        HashSet<GameObject> wallsInHits = new HashSet<GameObject>();
        List<RaycastHit> playerHits = new List<RaycastHit>();
        if(playerOneHits != null) playerHits.AddRange(playerOneHits);
        if(playerTwoHits != null) playerHits.AddRange(playerTwoHits);

        foreach (RaycastHit h in playerHits)
        {
            GameObject wall = h.collider.gameObject;

            if (!wallsInHits.Contains(wall))
            {
                wallsInHits.Add(wall);
            }
            Transparency trans = wall.GetComponent<Transparency>();
            if (trans == null)
            {
                trans = wall.AddComponent<Transparency>();
            }

            trans.TurnOnTransparency();

            if (!this.setOfWalls.Contains(wall))
            {
                this.setOfWalls.Add(wall);
            }
        }

        return wallsInHits;
    }

    private void RemoveWallsNotBetweenCameraAndPlayers(HashSet<GameObject> raycastedWalls)
    {
        HashSet<GameObject> setOfWallsCopy = new HashSet<GameObject>(this.setOfWalls);
        foreach(GameObject wall in setOfWallsCopy)
        {
            if(!raycastedWalls.Contains(wall))
            {
                this.setOfWalls.Remove(wall);
                wall.GetComponent<Transparency>().TurnOffTransparency();
            }
        }
    }

    private float mapRange(float s, float a1, float a2, float b1, float b2)
    {
        return (b1 + (s - a1) * (b2 - b1) / (a2 - a1));
    }
}
