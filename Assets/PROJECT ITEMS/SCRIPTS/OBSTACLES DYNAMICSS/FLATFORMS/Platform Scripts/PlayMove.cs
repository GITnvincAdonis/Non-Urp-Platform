using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    public enum MoveType
    {
        rotate,
        move,
    }
    // Start is called before the first frame update
    [SerializeField] Rigidbody Rigidbody = null;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private MoveType movementType;




    Vector3 prevPos = Vector3.zero;
    Vector3 currentPos = Vector3.zero;
   

    Dictionary<Rigidbody, float> RBsOnPlatfromAndTime = new Dictionary<Rigidbody, float>();
    [SerializeField] List<Rigidbody> RBsOnPlatform = new List<Rigidbody>();



    Dictionary<Movement, float> PlayerControlBodiesAndTime = new Dictionary<Movement, float>();
    [SerializeField] List<Movement> PlayerControlBodies = new List<Movement>();




    void FixedUpdate()
    {


        if (prevPos != transform.position)
        {
            prevPos = currentPos;
            //Debug.Log("previous position:" + prevPos);
            currentPos = transform.position;
            //Debug.Log(" position diff:" + (currentPos - prevPos));
        };
        

        if (RBsOnPlatform.Count != RBsOnPlatfromAndTime.Count)
        {
            RBsOnPlatfromAndTime.Clear();
            foreach (var rb in RBsOnPlatform)
            {
                RBsOnPlatfromAndTime.Add(rb, 1.0f);
            }
        }
        foreach (Rigidbody rb in RBsOnPlatform)
        {
            RBsOnPlatfromAndTime.TryGetValue(rb, out float value);
            if (value <1.0f)
            {
                RBsOnPlatfromAndTime[rb] += Time.deltaTime * 4.0f;
            }
            else if (value > 1.0f)
            {
                RBsOnPlatfromAndTime[rb] = 1.0f;
            }
            RotateAndMoveRBOnPlatform(rb, value);
        }




        ////////////////////////
        if (PlayerControlBodies.Count != PlayerControlBodiesAndTime.Count)
        {
            PlayerControlBodiesAndTime.Clear();
            foreach (var rb in PlayerControlBodies)
            {
                PlayerControlBodiesAndTime.Add(rb, 1.0f);
            }
        }
        foreach (Movement rb in PlayerControlBodies)
        {
            PlayerControlBodiesAndTime.TryGetValue(rb, out float value);
            if (value < 1.0f)
            {
                PlayerControlBodiesAndTime[rb] += Time.deltaTime * 4.0f;
            }
            else if (value > 1.0f)
            {
                PlayerControlBodiesAndTime[rb] = 1.0f;
            }
            RotateAndMoveControllersOnPlatform(rb, value);
        }
    }
    private void RotateAndMoveRBOnPlatform(Rigidbody rb, float timer)
    {
        if (movementType == MoveType.rotate)
        {
            float rotationAmount = _rotationSpeed * timer * Time.deltaTime;
            Quaternion localAngleAxis = Quaternion.AngleAxis(rotationAmount, Rigidbody.transform.up);
            rb.position = (localAngleAxis * (rb.position - Rigidbody.position)) + Rigidbody.position;

            Quaternion globalAngleAxis = Quaternion.AngleAxis(rotationAmount, rb.transform.InverseTransformDirection(Rigidbody.transform.up));
            rb.rotation *= globalAngleAxis;
        }
        else
        {
            rb.position += (currentPos - prevPos) * timer;
        }
    }


    private void RotateAndMoveControllersOnPlatform(Movement rb, float timer)
    {
        if (movementType == MoveType.rotate)
        {
            float rotationAmount = _rotationSpeed * timer * Time.deltaTime;
            Quaternion localAngleAxis = Quaternion.AngleAxis(rotationAmount, Rigidbody.transform.up);

            rb._controller.Move(((localAngleAxis * (rb.transform.position - Rigidbody.position)) + Rigidbody.position)- rb.transform.position);


            Quaternion globalAngleAxis = Quaternion.AngleAxis(rotationAmount, rb.transform.InverseTransformDirection(Rigidbody.transform.up));
            rb.transform.rotation *= globalAngleAxis;
        }
        else
        {
            rb._controller.Move((currentPos - prevPos) * timer);
            
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(!(other.attachedRigidbody ==null)&& !(other.attachedRigidbody.isKinematic))
        {
            if (!(RBsOnPlatform.Contains(other.attachedRigidbody)))
            {
                RBsOnPlatform.Add(other.attachedRigidbody);
                RBsOnPlatfromAndTime.Add(other.attachedRigidbody, 0.0f);
            }
        }


        if (!(other.GetComponent<Movement>() == null) )
        {
            Debug.Log("Entered");
            if (!(PlayerControlBodies.Contains(other.gameObject.GetComponent<Movement>())))
            {
                PlayerControlBodies.Add(other.gameObject.GetComponent<Movement>());
                PlayerControlBodiesAndTime.Add(other.gameObject.GetComponent<Movement>(), 0.0f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!(other.attachedRigidbody == null))
        {
            if (RBsOnPlatform.Contains(other.attachedRigidbody))
            {
                RBsOnPlatform.Remove(other.attachedRigidbody);
                RBsOnPlatfromAndTime.Remove(other.attachedRigidbody);
            }
        }

        if (!(other.GetComponent<Movement>() == null))
        {
            if (PlayerControlBodies.Contains(other.GetComponent<Movement>()))
            {
                PlayerControlBodies.Remove(other.gameObject.GetComponent<Movement>());
                PlayerControlBodiesAndTime.Remove(other.gameObject.GetComponent<Movement>());
            }
        }
    }
}
