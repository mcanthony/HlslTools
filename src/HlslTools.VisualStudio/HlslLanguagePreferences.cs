﻿using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;

namespace HlslTools.VisualStudio
{
    internal sealed class HlslLanguagePreferences : IVsTextManagerEvents2
    {
        private readonly HlslToolsPackage _package;
        private LANGPREFERENCES _preferences;

        public HlslLanguagePreferences(HlslToolsPackage package, LANGPREFERENCES preferences)
        {
            _package = package;
            _preferences = preferences;
        }

        public int OnRegisterMarkerType(int iMarkerType)
        {
            return VSConstants.S_OK;
        }

        public int OnRegisterView(IVsTextView pView)
        {
            return VSConstants.S_OK;
        }

        public int OnUnregisterView(IVsTextView pView)
        {
            return VSConstants.S_OK;
        }

        public int OnUserPreferencesChanged2(VIEWPREFERENCES2[] pViewPrefs, FRAMEPREFERENCES2[] pFramePrefs, LANGPREFERENCES2[] pLangPrefs, FONTCOLORPREFERENCES2[] pColorPrefs)
        {
            if (pLangPrefs != null && pLangPrefs.Length > 0 && pLangPrefs[0].guidLang == _preferences.guidLang)
            {
                _preferences.IndentStyle = pLangPrefs[0].IndentStyle;

                if (_preferences.fDropdownBar != (_preferences.fDropdownBar = pLangPrefs[0].fDropdownBar))
                {
                    foreach (var window in _package.CodeWindowManagers)
                    {
                        var hr = window.ToggleNavigationBar(_preferences.fDropdownBar != 0);
                        if (ErrorHandler.Failed(hr))
                        {
                            break;
                        }
                    }
                }
            }
            return VSConstants.S_OK;
        }

        public int OnReplaceAllInFilesBegin()
        {
            return VSConstants.S_OK;
        }

        public int OnReplaceAllInFilesEnd()
        {
            return VSConstants.S_OK;
        }

        public vsIndentStyle IndentMode => _preferences.IndentStyle;
        public bool NavigationBar => _preferences.fDropdownBar != 0;

        public int? SpacesPerIndent => _preferences.fInsertTabs == 0 ? (int?) _preferences.uIndentSize : null;
        public int TabSize => (int) _preferences.uTabSize;
    }
}