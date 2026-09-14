using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace ValheimPlus.Utility
{
    static class GameObjectAssistant
    {
        // TODO memory leak
        private static readonly ConcurrentDictionary<float, Stopwatch> Stopwatches = new();

        private static readonly MethodInfo InvokeRepeatingMethod =
            AccessTools.Method(typeof(MonoBehaviour), nameof(MonoBehaviour.InvokeRepeating));

        public static Stopwatch GetStopwatch(GameObject o)
        {
            var hash = GetGameObjectPositionHash(o);
            if (Stopwatches.TryGetValue(hash, out var stopwatch)) return stopwatch;

            stopwatch = new Stopwatch();
            Stopwatches.TryAdd(hash, stopwatch);
            return stopwatch;
        }

        public static float GetGameObjectPositionHash(GameObject obj)
        {
            var position = obj.transform.position;
            return 1000f * position.x + position.y + .001f * position.z;
        }

        private const float ChestPollInterval = 1f;
        private const float MaxChestWait = 10f;

        /// <summary>
        /// Points every InvokeRepeating(methodName, ...) call at replacement, a static (machine, methodName, delay, rate)
        /// stand-in, so vanilla's arguments stay as written. Only the arguments sit between the name and its call.
        /// </summary>
        public static IEnumerable<CodeInstruction> ReplaceInvokeRepeating(IEnumerable<CodeInstruction> instructions,
            string patchName, string methodName, MethodInfo replacement) =>
            new CodeMatcher(instructions)
                .MatchStartForward(new CodeMatch(OpCodes.Ldstr, methodName))
                .Repeat(
                    matcher => matcher
                        .SearchForward(instruction => instruction.Calls(InvokeRepeatingMethod))
                        .SetOperandAndAdvance(replacement),
                    _ => PatchLog.Failed(patchName, $"{methodName} will not wait for nearby chests to load before auto-depositing."))
                .InstructionEnumeration();

        /// <summary>InvokeRepeating, held until chests within chestRange have loaded. A range of 0 doesn't wait.</summary>
        public static void InvokeRepeatingWhenChestsLoaded(MonoBehaviour machine, string methodName, float delay,
            float rate, float chestRange)
        {
            if (chestRange <= 0f || InventoryAssistant.NearbyChestsLoaded(machine.transform.position, chestRange))
                machine.InvokeRepeating(methodName, delay, rate);
            else
                machine.StartCoroutine(InvokeRepeatingOnceChestsLoaded(machine, methodName, delay, rate, chestRange));
        }

        // Stops with the machine if it's destroyed while waiting.
        private static IEnumerator InvokeRepeatingOnceChestsLoaded(MonoBehaviour machine, string methodName,
            float delay, float rate, float chestRange)
        {
            float waited = 0f;
            for (; waited < MaxChestWait && !InventoryAssistant.NearbyChestsLoaded(machine.transform.position, chestRange);
                 waited += ChestPollInterval)
                yield return new WaitForSeconds(ChestPollInterval);

            // Rare, and explains output landing on the ground right after a load.
            if (waited >= MaxChestWait && !InventoryAssistant.NearbyChestsLoaded(machine.transform.position, chestRange))
                ValheimPlusPlugin.Logger.LogDebug(
                    $"{machine.name} at {machine.transform.position} gave up waiting {MaxChestWait}s for nearby chests to load, starting {methodName} anyway.");

            machine.InvokeRepeating(methodName, delay, rate);
        }

        public static T GetChildComponentByName<T>(string name, GameObject objected) where T : Component
        {
            foreach (var component in objected.GetComponentsInChildren<T>(true))
            {
                if (component.gameObject.name == name)
                {
                    return component;
                }
            }
            return null;
        }
    }
}
