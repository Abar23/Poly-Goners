using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    public ParticleSystem Main;
    public ParticleSystem[] Ring;
    public SparkParticles Sparks;
    public ParticleSystem Flash;
    public ParticleSystem[] Debris;


    public void Play()
    {
        GameObject newExplosion = Instantiate(this.gameObject, this.gameObject.transform.position, Quaternion.identity);

        newExplosion.GetComponent<Explosion>().Explode();
    }

    public void Explode()
    {
        StartCoroutine(WaitPlay());
        StartCoroutine(DestroyObject());
    }

    private IEnumerator WaitPlay()
    {
        yield return new WaitForSeconds(1.9f);

        //Main.Play();
        for (int i = 0; i < Ring.Length; i++)
            Ring[i].Play();
        Sparks.Play();
        Flash.Play();
        for (int i = 0; i < Debris.Length; i++)
            Debris[i].Play();
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}

