using UnityEngine;

public class UIUnitSelectionManager : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;


    private void Awake()
    {
        rectTransform.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitSelectionManager.Instance.OnSelectionAreaStart += Instance_OnSelectionAreaStart;
        UnitSelectionManager.Instance.OnSelectionAreaEnd += Instance_OnSelectionAreaEnd;
    }

    private void Update()
    {
        if(rectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }


    private void Instance_OnSelectionAreaStart(object sender, System.EventArgs e)
    {
       rectTransform.gameObject.SetActive(true);

        UpdateVisual();
    }

    private void Instance_OnSelectionAreaEnd(object sender, System.EventArgs e)
    {
        rectTransform.gameObject.SetActive(false);
    }

    

    public void UpdateVisual()
    {
        Rect selectionAreaRect = UnitSelectionManager.Instance.GetSelectionAreaRect();

        rectTransform.anchoredPosition = new Vector2(selectionAreaRect.x,selectionAreaRect.y);
        rectTransform.sizeDelta = new Vector2(selectionAreaRect.width,selectionAreaRect.height);
    }
}
