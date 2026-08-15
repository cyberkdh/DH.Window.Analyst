//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst.UI
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace DH.Window.Analyst.UI.Utils {
	// shared row-interaction wiring for any ListView that offers right-click "Copy"/"Export All...": Ctrl+A select-all,
	// right-click preserves an existing multi-selection (Windows convention), "Copy" hidden when nothing is selected
	// so an empty selection only ever offers "Export All...", and "Export All..." always exports every row via
	// ListViewExportHelper regardless of selection.
	public static class ListViewRowInteractionHelper {
		// strcopyaction: null uses ListViewExportHelper.CopyToClipboard (tab-delimited full row); pass a custom
		// action for controls that need a different clipboard format (e.g. PropertyViewControl's "Name: Value" style)
		public static void AttachRowContextMenu(ListView lsv, string strexportbasename, Action copyaction = null) {
			Action copyactionresolved = copyaction ?? (() => ListViewExportHelper.CopyToClipboard(lsv));

			ToolStripMenuItem itemcopy = new ToolStripMenuItem("Copy");
			itemcopy.Click += (sender, e) => copyactionresolved();

			ToolStripMenuItem itemexportall = new ToolStripMenuItem("Export All...");
			itemexportall.Click += (sender, e) => ListViewExportHelper.ExportAll(lsv, strexportbasename);

			ContextMenuStrip menu = new ContextMenuStrip();
			menu.Items.Add(itemcopy);
			menu.Items.Add(itemexportall);
			menu.Opening += (sender, e) => { itemcopy.Visible = lsv.SelectedItems.Count > 0; };
			lsv.ContextMenuStrip = menu;

			// right-click on a row that's already part of the current selection keeps that selection, so Ctrl+A then
			// right-click still offers "Copy" for every selected row; right-click on an unselected row replaces the
			// selection with just that row
			lsv.MouseDown += (sender, e) => {
				if (e.Button != MouseButtons.Right) {
					return;
				}

				ListViewItem itemhit = lsv.GetItemAt(e.X, e.Y);
				if (itemhit == null) {
					return;
				}

				if (itemhit.Selected == true) {
					return;
				}

				lsv.SelectedItems.Clear();
				itemhit.Selected = true;
				itemhit.Focused = true;
			};

			AttachSelectAllShortcut(lsv);
		}

		public static void AttachSelectAllShortcut(ListView lsv) {
			// without this, Ctrl+A can be swallowed before KeyDown ever fires — ListView doesn't treat a plain
			// letter key as an "input key" by default, so it's routed away as a possible dialog/menu shortcut first
			lsv.PreviewKeyDown += (sender, e) => {
				if (e.Control == true && e.KeyCode == Keys.A) {
					e.IsInputKey = true;
				}
			};

			lsv.KeyDown += (sender, e) => {
				if (e.Control == true && e.KeyCode == Keys.A) {
					foreach (ListViewItem lvitem in lsv.Items) {
						lvitem.Selected = true;
					}
					e.Handled = true;
				}
			};
		}
	}
}
