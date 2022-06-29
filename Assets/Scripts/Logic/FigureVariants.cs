using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SO/FigureVariants", fileName = "FigureVariants")]
public class FigureVariants : ScriptableObject
{
	public List<FigureVariant> variants = new List<FigureVariant>();
}

