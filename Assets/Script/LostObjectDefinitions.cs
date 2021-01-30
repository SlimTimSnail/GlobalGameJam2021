using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LostObjectDefinitions", menuName = "GGJ2021/LostObjectDefinitions")]
public class LostObjectDefinitions : ScriptableObject
{
	public enum Size
	{
		Small,
		Medium,
		Large
	}

	public enum Colour
	{
		White,
		Black,
		Blue,
		Green,
		Yellow,
		Pink,
		Silver
	}

	public enum Category
	{
		Food,
		Electronic,
		Tool,
		Toy
	}

	[Serializable]
	public struct LostObjectDefinition
	{
		public string Name;

		public Size Size;

		public Colour Colour;

		public Category Category;
	}

	public List<LostObjectDefinition> Definitions = new List<LostObjectDefinition>();
}
