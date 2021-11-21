using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectScaleSprites : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
	private List<Vector3> objectsArea;
	private float restSmoothTime;
	private float timeToBeat;
	protected bool m_isBeat;
	private Vector3 restScale;
    private void Start()
    {
		objectsArea = new List<Vector3>();
		float sizeOfBeat;
		restScale = new Vector3(1, 1, 1);
		for(int i = 0; i<objects.Count; i++)
        {
			sizeOfBeat=1+(1/objects[i].GetComponent<SpriteRenderer>().bounds.size.x*objects[i].GetComponent<SpriteRenderer>().bounds.size.y)/5;
			objectsArea.Add(new Vector3(sizeOfBeat,sizeOfBeat,1));
			
        }
		restSmoothTime = 20;
		timeToBeat = 0.3f;
	}
    private void Update()
	{
		OnUpdate();
	}
	private void OnUpdate()
	{
		if (m_isBeat) return;
		for (int i = 0; i < objects.Count; i++)
		{
			//Sprite return to normal size
			objects[i].transform.localScale = Vector3.Lerp(objects[i].transform.localScale, restScale, restSmoothTime * Time.deltaTime);
		}
	}
	public void OnBeat()
	{
		m_isBeat = true;
		for(int i = 0; i < objects.Count; i++)
        {
			StopCoroutine("MoveToScale");
			StartCoroutine("MoveToScale", objectsArea[i]);
		}
		m_isBeat = false;
	}

	private IEnumerator MoveToScale(Vector3 _target)
	{
		Vector3 _curr = transform.localScale;
		Vector3 _initial = _curr;
		float _timer = 0;

		while (_curr != _target)
		{
			_curr = Vector3.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;
			for (int i = 0; i < objects.Count; i++)
			{
				//Sprite scale
				objects[i].transform.localScale = _curr;
			}
			//transform.localScale = _curr;

			yield return null;
		}

		//m_isBeat = false;

	}
}




///
//Vector3 _target;
//Vector3 _curr = transform.localScale;
//Vector3 _initial = _curr;
//float _timer = 0;
//for (int i = 0; i < objects.Count; i++)
//{
//	_target = objectsArea[i];
//	while (_curr != _target)
//	{
//		_curr = Vector3.Lerp(_initial, _target, _timer / timeToBeat);
//		_timer += Time.deltaTime;
//		//Sprite scale
//		objects[i].transform.localScale = _curr;
//		//transform.localScale = _curr;

//		yield return null;
//	}
//}
//m_isBeat = false;