using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Capibutler.Utils
{
    [Serializable]
    public struct StartUpAction
    {
        public float delayedBySeconds;
        public UnityEvent action;
    }

    public class ExecuteAtStart : MonoBehaviour
    {
        public List<StartUpAction> actions;
        private IOrderedEnumerable<StartUpAction> sortedActions;

        private void Awake()
        {
            sortedActions = actions.OrderBy(action => action.delayedBySeconds);
        }

        private IEnumerator Start()
        {
            foreach (var actionItem in sortedActions) {
                var currentTime = Time.timeSinceLevelLoad;
                if (currentTime < actionItem.delayedBySeconds)
                    yield return new WaitForSecondsRealtime(actionItem.delayedBySeconds - currentTime);

                actionItem.action?.Invoke();
            }

            // No children and only Component on the gameObject (+transform) => destroy the whole thing
            // gameObject has children and/or other Components => just remove itself 
            Destroy(transform.childCount == 0 && gameObject.GetComponents<Component>().Length == 2 ? gameObject : this);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}