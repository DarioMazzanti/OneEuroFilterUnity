/* 
 * FilterTestVector3.cs
 * Author: Dario Mazzanti (dario.mazzanti@iit.it), 2016
 * 
 * Testing OneEuroFilter utility on a Unity Vector3
 *
 */

using UnityEngine;
using System.Collections;

public class FilterTestVector3 : MonoBehaviour 
{
	public Transform noisyTransform;
	public Transform filteredTransform;

	Vector3 startingPosition;
	Vector3 offset;

	OneEuroFilter<Vector3> positionFilter;

	public bool filterOn = true;

	public float filterFrequency = 120.0f;
	public float filterMinCutoff = 1.0f;
	public float filterBeta = 0.0f;
	public float filterDcutoff = 1.0f;

	public float noiseAmount = 1f;

	public float oscillationSpeed  = 0.025f;
	float angle  = 0.0f;

	void Start () 
	{
		positionFilter = new OneEuroFilter<Vector3>(filterFrequency);
		startingPosition = noisyTransform.position;

		offset = filteredTransform.position - noisyTransform.position;
	}

	void Update () 
	{
		noisyTransform.position = PerturbedPosition(startingPosition) + Oscillation();

		if(filterOn)
		{	
			positionFilter.UpdateParams(filterFrequency, filterMinCutoff, filterBeta, filterDcutoff);
			filteredTransform.position = positionFilter.Filter(noisyTransform.position) + offset;
		}
		else
			filteredTransform.position = noisyTransform.position + offset;
	}

	Vector3 PerturbedPosition(Vector3 _position)
	{
		Vector3 noise = new Vector3(Random.value*noiseAmount - noiseAmount/2.0f, Random.value*noiseAmount - noiseAmount/2.0f, Random.value*noiseAmount - noiseAmount/2.0f)*Time.deltaTime;

		return _position + noise;
	}

	Vector3 Oscillation()
	{
		angle += oscillationSpeed*Time.deltaTime;
		if(angle == 360f)
			angle = 0f;		
		
		return new  Vector3(0f, Mathf.Sin(angle), 0f);
	}
}