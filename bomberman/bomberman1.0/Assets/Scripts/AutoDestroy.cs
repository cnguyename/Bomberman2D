using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    public float time;

	private AudioSource source;
	public AudioClip explosion_sound;

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

    IEnumerator Start() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
