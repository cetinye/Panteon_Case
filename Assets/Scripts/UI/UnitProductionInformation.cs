using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrategyGameDemo.UI
{
	public class UnitProductionInformation : MonoBehaviour
	{
		[SerializeField] private TMP_Text unitName;
		[SerializeField] private Image unitImage;

		public void Initialize(string name, Sprite image)
		{
			unitName.text = name;
			unitImage.sprite = image;
		}
	}
}