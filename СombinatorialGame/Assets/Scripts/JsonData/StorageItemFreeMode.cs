using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageItemFreeMode
{
    [JsonProperty(PropertyName = "R")]
    public int Round { get; set; }
}
