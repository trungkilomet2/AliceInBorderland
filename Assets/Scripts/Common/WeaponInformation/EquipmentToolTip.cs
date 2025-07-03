using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipmentTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string equipmentInfo; // Thông tin sẽ hiển thị

    public TextMeshProUGUI tooltipText; // Text bên trong panel

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = equipmentInfo;

    }

    public void SetEquipmentInfo(string info)
    {
        equipmentInfo = info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
