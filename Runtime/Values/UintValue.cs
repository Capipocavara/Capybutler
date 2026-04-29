using System.Collections.Generic;
using Capibutler.Values.Base;
using UnityEngine;

namespace Capibutler.Values
{
    [CreateAssetMenu(fileName = "UintValue", menuName = "Voodoo/Values/UintValue")]
    public class UintValue : GenericValue<uint> { }

    public class MyCustom : List<Dictionary<float, int>> { }
}