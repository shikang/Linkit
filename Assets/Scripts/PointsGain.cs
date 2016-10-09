﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointsGain : MonoBehaviour
{
	public const float LIFETIME = 1.0f;
	const float Y_SPEED = 1.0f;

	Text m_Text;
	// Use this for initialization
	void Start ()
	{
		m_Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Position
		Vector3 pos = transform.position;
		pos.y += Y_SPEED * Time.deltaTime;
		transform.position = pos;

		// Colour
		Color c = m_Text.color;
		c.a = c.a - LIFETIME * Time.deltaTime;
		m_Text.color = c;
	}
}
