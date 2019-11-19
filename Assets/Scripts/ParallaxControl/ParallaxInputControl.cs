using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxInputControl : MonoBehaviour
{
    public ParallaxControl parallaxCtrl;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // This is bad, but for the sake of this prototype it is what it is
	    // It would be better to get the reference in an other way than manual
	    // like searching for it.
    }

    // Update is called once per frame
    void Update()
    {
        if (parallaxCtrl != null)
		{
			//parallaxCtrl.Speed = Input.GetAxisRaw("Horizontal") * runSpeed * -1;
            parallaxCtrl.Speed =  mainCamera.velocity.magnitude * -1.0f;
            //Debug.Log(mainCamera.velocity.magnitude);
		}
    }
}
