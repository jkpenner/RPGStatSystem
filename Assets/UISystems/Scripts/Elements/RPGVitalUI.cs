using UnityEngine;
using UnityEngine.UI;


public class RPGVitalUI : MonoBehaviour {
    public RectTransform activeFill;
    public Text values;

    public void SetActiveFill(float value, float max) {
        activeFill.localScale = new Vector3(
            Mathf.Clamp(Mathf.Max(value, 0) / Mathf.Max(max, 1), 0f, 1f),
            activeFill.localScale.y,
            activeFill.localScale.z);

        if (values != null) {
            values.text = string.Format("{0}/{1}", Mathf.Max(value, 0), Mathf.Max(max, 1));
        }
    }
}