using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current;
    [Header("Movement System")]
    public float xLimit;
    public float horizontalSpeed, runningSpeed;

    private float _lastTouchedX;
    private float _currentRunningSpeed;
    [Header("Creat Cube System")]
    public List<AddingCube> cubes;
    public GameObject cubePrefab;
    public Animator animator;
    
    
    

    void Update()
    {
        if (!LevelController.Current.gameActive)
        {
            return;
        }
        float newX;
        float touchDeltaX = 0;
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _lastTouchedX = Input.GetTouch(0).position.x;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchDeltaX = 5 * (Input.GetTouch(0).position.x - _lastTouchedX) / Screen.width;
                _lastTouchedX = Input.GetTouch(0).position.x;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            touchDeltaX = Input.GetAxis("Mouse X");
        }
        newX = transform.position.x + horizontalSpeed * touchDeltaX * Time.deltaTime;
        newX = Mathf.Clamp(newX, -xLimit, xLimit);

        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + _currentRunningSpeed * Time.deltaTime);
        transform.position = newPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AddCube"))
        {
            int _unit = other.gameObject.GetComponent<CubeCount>().unit;
            for (int i = 0; i < _unit; i++)
            {
                CreateCube(true);
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            other.tag = "Untagged";
            int _unit = other.gameObject.GetComponent<Trap>().unit;
            for (int i = 0; i < _unit; i++)
            {
                CreateCube(false);
            }
        }
        

    }

   
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            if (cubes.Count > 0)
            {
                return;
            }
            else
            {
                LevelController.Current.FinishGame(collision.gameObject.GetComponent<FinishBonus>().bonus);
            }
        }
    }

    public void ChangeSpeed(float value)
    {
        _currentRunningSpeed = value;
    }

    public void CreateCube(bool add)
    {
        if (add)
        {
            AddingCube addedCube = Instantiate(cubePrefab, transform).GetComponent<AddingCube>();
            cubes.Add(addedCube);
            addedCube.AddCube(add);
        }
        else
        {
            cubes[cubes.Count - 1].DestroyCube();
        }
    }


    public void DestroyCube(AddingCube addingCube)
    {

        cubes.Remove(addingCube);
        addingCube.transform.SetParent(null);


    }

    public void Die()
    {
        LevelController.Current.GameOver();
    }
   

}
