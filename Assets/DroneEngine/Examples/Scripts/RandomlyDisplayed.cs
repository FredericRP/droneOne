using FredericRP.BucketGenerator;
using System.Collections.Generic;
using UnityEngine;


namespace FredericRP.DroneEngine
{
  public class RandomlyDisplayed : MonoBehaviour
  {
    [SerializeField]
    [Range(0, 100)]
    int displayPercentage = 50;
    [SerializeField]
    string identifier = "random";

    static Dictionary<string, Bucket> bucketDict;

    // Start is called before the first frame update
    void Start()
    {
      if (bucketDict == null)
        bucketDict = new Dictionary<string, Bucket>();
      Bucket bucket;
      if (!bucketDict.ContainsKey(identifier))
      {
        bucketDict.Add(identifier, new Bucket(100));
      }
      bucket = bucketDict[identifier];
      gameObject.SetActive(bucket.GetRandomNumber() < displayPercentage);
    }

  }
}