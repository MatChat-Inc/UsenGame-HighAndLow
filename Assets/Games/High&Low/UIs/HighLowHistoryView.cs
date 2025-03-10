
using System.Collections.Generic;
using System.Linq;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class HighLowHistoryView: Widget
{
    public Text restCountLabel;
    
    [HideInInspector] public List<int> pokers;
    
    public void Start()
    {
        foreach (var poker in pokers)
        {
            var itemPath = string.Format("CheckList/{0}", EPokersHelper.GetTextureNameFromPoker((EPokers)poker));
            transform.Find(itemPath).gameObject.GetComponent<Image>().color = Color.white;
        }
    
        restCountLabel.text = string.Format("{0}", 52 - pokers.Count());
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
            Navigator.Pop();
    }
}
