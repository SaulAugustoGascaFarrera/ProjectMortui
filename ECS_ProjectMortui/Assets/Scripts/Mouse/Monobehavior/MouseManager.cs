using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;    
    private void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetMousePosition()
    { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if(Physics.Raycast(ray,out RaycastHit hit,float.MaxValue,1 << 6))
        {
            return hit.point;
        }

        return Vector3.zero;  
    }

}
