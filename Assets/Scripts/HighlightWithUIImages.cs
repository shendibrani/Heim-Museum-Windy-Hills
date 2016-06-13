using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighlightWithUIImages : Highlightable
{
	[SerializeField] List<Image> images;

	public override void ProcessHighlight(){
		if(highlighted){
			foreach(Image i in images){
				i.color = Color.white;
			}
		} else {
			foreach(Image i in images){
				i.color = Color.clear;
			}
		}
	}
}


