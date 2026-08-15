//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using DH.Window.Analyst.Logging;

namespace DH.Window.Analyst.Settings {
	// singleton facade over HKCU\SOFTWARE\DHTOOL\DH.Window.Analyst; falls back to a hardcoded default when a registry value is absent/unreadable
	public class AppSettings {
		private static readonly AppSettings s_instance = new AppSettings();

		public static readonly string REGISTRY_SUBKEY_PATH = @"SOFTWARE\DHTOOL\DH.Window.Analyst";

		private const string KEY_GROUP_BY_PROCESS_DEFAULT = "general_group_by_process_default";
		private const string KEY_REMEMBER_MAIN_WINDOW_BOUNDS = "general_remember_main_window_bounds";
		private const string KEY_MAIN_WINDOW_X = "general_main_window_x";
		private const string KEY_MAIN_WINDOW_Y = "general_main_window_y";
		private const string KEY_MAIN_WINDOW_WIDTH = "general_main_window_width";
		private const string KEY_MAIN_WINDOW_HEIGHT = "general_main_window_height";

		private const string KEY_LOG_LEVEL = "logging_level";
		private const string KEY_LOG_RETENTION_DAYS = "logging_retention_days";

		private const string KEY_HIGHLIGHT_HOLD_MS = "inspector_highlight_hold_ms";
		private const string KEY_HIGHLIGHT_FADE_MS = "inspector_highlight_fade_ms";
		private const string KEY_HIGHLIGHT_COLOR_ARGB = "inspector_highlight_color_argb";
		private const string KEY_ACCESSIBILITY_MAX_ELEMENTS = "inspector_accessibility_max_elements";

		public const bool DEFAULT_GROUP_BY_PROCESS = true;
		public const bool DEFAULT_REMEMBER_MAIN_WINDOW_BOUNDS = false;
		public const LogLevel DEFAULT_LOG_LEVEL = LogLevel.All;
		public const int DEFAULT_LOG_RETENTION_DAYS = 7;
		public const int DEFAULT_HIGHLIGHT_HOLD_MS = 250;
		public const int DEFAULT_HIGHLIGHT_FADE_MS = 400;
		public static readonly Color DEFAULT_HIGHLIGHT_COLOR = Color.OrangeRed;
		public const int DEFAULT_ACCESSIBILITY_MAX_ELEMENTS = 5000;

		private readonly RegistrySettingsStore m_store = new RegistrySettingsStore(REGISTRY_SUBKEY_PATH);

		private AppSettings() {
		}

		public static AppSettings Get() {
			return s_instance;
		}

		public bool GroupByProcessDefault {
			get { return m_store.GetBool(KEY_GROUP_BY_PROCESS_DEFAULT, DEFAULT_GROUP_BY_PROCESS); }
			set { m_store.SetBool(KEY_GROUP_BY_PROCESS_DEFAULT, value); }
		}

		public bool RememberMainWindowBounds {
			get { return m_store.GetBool(KEY_REMEMBER_MAIN_WINDOW_BOUNDS, DEFAULT_REMEMBER_MAIN_WINDOW_BOUNDS); }
			set { m_store.SetBool(KEY_REMEMBER_MAIN_WINDOW_BOUNDS, value); }
		}

		public Rectangle? GetMainWindowBounds() {
			int nwidth = m_store.GetInt(KEY_MAIN_WINDOW_WIDTH, 0);
			int nheight = m_store.GetInt(KEY_MAIN_WINDOW_HEIGHT, 0);
			if (nwidth <= 0 || nheight <= 0) {
				return null;
			}

			int nx = m_store.GetInt(KEY_MAIN_WINDOW_X, 0);
			int ny = m_store.GetInt(KEY_MAIN_WINDOW_Y, 0);
			return new Rectangle(nx, ny, nwidth, nheight);
		}

		public void SetMainWindowBounds(Rectangle rectbounds) {
			m_store.SetInt(KEY_MAIN_WINDOW_X, rectbounds.X);
			m_store.SetInt(KEY_MAIN_WINDOW_Y, rectbounds.Y);
			m_store.SetInt(KEY_MAIN_WINDOW_WIDTH, rectbounds.Width);
			m_store.SetInt(KEY_MAIN_WINDOW_HEIGHT, rectbounds.Height);
		}

		public LogLevel LogLevel {
			get { return (LogLevel) m_store.GetInt(KEY_LOG_LEVEL, (int) DEFAULT_LOG_LEVEL); }
			set { m_store.SetInt(KEY_LOG_LEVEL, (int) value); }
		}

		public int LogRetentionDays {
			get { return m_store.GetInt(KEY_LOG_RETENTION_DAYS, DEFAULT_LOG_RETENTION_DAYS); }
			set { m_store.SetInt(KEY_LOG_RETENTION_DAYS, value); }
		}

		public int HighlightHoldMs {
			get { return m_store.GetInt(KEY_HIGHLIGHT_HOLD_MS, DEFAULT_HIGHLIGHT_HOLD_MS); }
			set { m_store.SetInt(KEY_HIGHLIGHT_HOLD_MS, value); }
		}

		public int HighlightFadeMs {
			get { return m_store.GetInt(KEY_HIGHLIGHT_FADE_MS, DEFAULT_HIGHLIGHT_FADE_MS); }
			set { m_store.SetInt(KEY_HIGHLIGHT_FADE_MS, value); }
		}

		public Color HighlightColor {
			get { return Color.FromArgb(m_store.GetInt(KEY_HIGHLIGHT_COLOR_ARGB, DEFAULT_HIGHLIGHT_COLOR.ToArgb())); }
			set { m_store.SetInt(KEY_HIGHLIGHT_COLOR_ARGB, value.ToArgb()); }
		}

		public int AccessibilityMaxElements {
			get { return m_store.GetInt(KEY_ACCESSIBILITY_MAX_ELEMENTS, DEFAULT_ACCESSIBILITY_MAX_ELEMENTS); }
			set { m_store.SetInt(KEY_ACCESSIBILITY_MAX_ELEMENTS, value); }
		}
	}
}
