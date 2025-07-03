using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string equimmentName; // Tên thiết bị
    private string equipmentInfo; // Thông tin sẽ hiển thị

    public TextMeshProUGUI tooltipText; // Text bên trong panel
    public TextMeshProUGUI tooltipName; // Text bên trong panel


    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = equipmentInfo;
        tooltipName.text = equimmentName;
    }

    public void SetEquipmentInfo(string info)
    {
        this.equipmentInfo = info;
    }

    public void SetEquipmentName(string name)
    {
        this.equimmentName = name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.text = "Hold to see information!!!";
        tooltipName.text = "";
    }
}
