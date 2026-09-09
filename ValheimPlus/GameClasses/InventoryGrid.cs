using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using ValheimPlus.Configurations;

namespace ValheimPlus.GameClasses
{
    [HarmonyPatch(typeof(InventoryGrid), nameof(InventoryGrid.UpdateGui))]
    public static class InventoryGrid_UpdateGui_Patch
    {
        /// <summary>
        /// Fixes bug where m_elements is only re-filled when width or height changes.
        /// InventoryGrid is instantiated with a certain width/height, and with an empty m_elements.
        /// If width and height both match the inventory width/height when we get to UpdateGui,
        /// then m_elements is allowed to be an empty list. However, we need m_elements to be of
        /// size `width * height`, so we must force those conditions to be false ourselves. 
        /// </summary>
        [UsedImplicitly]
        private static void Prefix(InventoryGrid __instance)
        {
            LayoutContainerScrollbar(__instance);

            int width = __instance.m_inventory.GetWidth();
            int height = __instance.m_inventory.GetHeight();
            
            // Our bug won't trigger, continue with method as normal
            if (__instance.m_width != width || 
                __instance.m_height != height ||
                __instance.m_elements.Count == width * height) return;
            
            // Our bug is about to occur, break one of the conditions to make sure m_elements is re-created.
            // Change m_width to either one or two based on whether the current is already 1 or not.
            // m_width is always set to m_inventory.GetWidth() in this method anyways.
            __instance.m_width = __instance.m_width == 1 ? 2 : 1;
        }

        /// <summary>Gap left around the item columns and the container scrollbar.</summary>
        private const float scrollbarPadding = 8f;

        /// <summary>Container geometry as the game laid it out, captured before we touch it.</summary>
        private static float basePanelWidth = float.NaN;
        private static float baseGridInset;
        private static float baseBarOffset;

        /// <summary>
        /// Make room for the container scrollbar next to a widened item grid. The game lays the bar
        /// out for a stock-width chest, so wider columns run out underneath it. Widening the panel
        /// alone does not help - the grid stretches to the panel and UpdateGui centres the items in
        /// the grid - so reserve a strip on the right of the grid and centre the items in the rest.
        /// </summary>
        private static void LayoutContainerScrollbar(InventoryGrid grid)
        {
            if (!Configuration.Current.Inventory.IsEnabled) return;

            InventoryGui gui = InventoryGui.instance;
            if (gui == null || grid != gui.m_containerGrid || grid.m_inventory == null) return;

            Scrollbar scrollbar = grid.m_scrollbar;
            RectTransform panel = gui.m_container;
            RectTransform gridRect = grid.gameObject.GetComponent<RectTransform>();
            RectTransform bar = scrollbar == null ? null : scrollbar.transform as RectTransform;
            if (bar == null || panel == null || gridRect == null) return;

            float scale = panel.lossyScale.x;
            if (scale <= 0f) return;

            float items = grid.m_inventory.GetWidth() * grid.m_elementSpace * scale;
            if (items <= 0f) return;

            if (float.IsNaN(basePanelWidth))
            {
                basePanelWidth = panel.sizeDelta.x;
                baseGridInset = gridRect.offsetMax.x;
                baseBarOffset = bar.anchoredPosition.x;
            }

            float pad = scrollbarPadding * scale;
            float barWidth = bar.sizeDelta.x * scale;

            // Left margin, items, gap, bar, edge margin.
            float needed = items + 3f * pad + barWidth;

            // A stock-width container already leaves the bar room. The same grid and panel serve
            // every container, so restore the layout rather than just skipping.
            if (needed <= basePanelWidth * scale)
            {
                Restore(gridRect, panel, bar);
                return;
            }

            // Bar plus its margin. The items-to-bar gap comes out of the grid's own centring.
            float strip = barWidth + pad;

            // Hold the strip back from the grid so the items centre to the left of it.
            Set(gridRect, panel, bar,
                inset: -strip / scale,
                width: needed / scale,
                // The bar is anchored to the panel centre, so offset it from there.
                barOffset: (needed / 2f - pad - barWidth / 2f) / scale);
        }

        private static void Restore(RectTransform gridRect, RectTransform panel, RectTransform bar) =>
            Set(gridRect, panel, bar, baseGridInset, basePanelWidth, baseBarOffset);

        private static void Set(RectTransform gridRect, RectTransform panel, RectTransform bar,
            float inset, float width, float barOffset)
        {
            if (!Mathf.Approximately(gridRect.offsetMax.x, inset))
                gridRect.offsetMax = new Vector2(inset, gridRect.offsetMax.y);

            if (!Mathf.Approximately(panel.sizeDelta.x, width))
                panel.sizeDelta = new Vector2(width, panel.sizeDelta.y);

            if (!Mathf.Approximately(bar.anchoredPosition.x, barOffset))
                bar.anchoredPosition = new Vector2(barOffset, bar.anchoredPosition.y);
        }
    }
}
