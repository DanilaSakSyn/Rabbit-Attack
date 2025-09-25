using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace XCodeBuildFixes
{
    internal static class FixBuildPhasesOrderForUnityIPhone
    {
        private const string _sectionSearchKey = "Build configuration list for PBXNativeTarget \"Unity-iPhone\"";
        private const string _targetShellScriptSectionKey = "Begin PBXShellScriptBuildPhase section";
        private const string _targetEndShellScriptSectionKey = "End PBXShellScriptBuildPhase section";

        [PostProcessBuild(999)]
        private static void FixBuildPhasesOrderInProject(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }

            var path = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj", "project.pbxproj");
            var allLines = File.ReadAllLines(path);

            var id = FindShellScriptId(allLines);
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("Can not find ID for 'Unity Process symbols'.");
                return;
            }

// Search for order array.
// Search for first line.
            var firstLine = -1;
            for (var i = 0; i + 2 < allLines.Length; i++)
            {
                if (allLines[i].Contains(_sectionSearchKey) && allLines[i + 1].Contains("buildPhases = ("))
                {
                    firstLine = i + 2;
                    break;
                }
            }

// Check for error.
            if (firstLine <= 0)
            {
                return;
            }

// Search for last line.
            var lastLine = -1;
            for (var i = firstLine; i + 1 < allLines.Length && i - firstLine < 30; i++)
            {
                if (allLines[i + 1].Contains(");"))
                {
                    lastLine = i;
                    break;
                }
            }

// Check for error.
            if (lastLine <= 0 || lastLine == firstLine)
            {
                return;
            }

            var buffer = new List<string>();
            for (var i = firstLine; i <= lastLine; i++)
            {
                buffer.Add(allLines[i]);
            }

// Check order of Sources.
            var index = buffer.FindIndex(x => x.Contains(id));
            if (index == -1)
            {
                return;
            }

            var line = buffer[index];
            buffer.RemoveAt(index);
            buffer.Add(line);

// Replace old lines with new ones.
            for (var i = 0; i < buffer.Count; i++)
            {
                allLines[firstLine + i] = buffer[i];
            }

// Save results.
            File.WriteAllLines(path, allLines);
            Debug.Log("BuildPhases order were reordered in xCode project.");
        }

        private static string FindShellScriptId(string[] allLines)
        {
            var section = FindShellScriptSection(allLines);
            if (section == null)
            {
                Debug.LogError("Can not find PBXShellScriptBuildPhase section.");
                return null;
            }

// posible encounters:
// name = Unity Process symbols - old Unity ~2020
// name = "Unity Process symbols for Unity-iPhone"; - newer Unity ~2022
// name = "Unity Process symbols for UnityFramework"; - newer Unity ~2022
            var nameIndex = -1;
            for (var i = section.Value.SectionStart; i < section.Value.SectionEnd; i++)
            {
                if (allLines[i].Contains("Unity Process symbols") && !allLines[i].Contains("UnityFramework"))
                {
                    nameIndex = i;
                    break;
                }
            }

            if (nameIndex == -1)
            {
                Debug.LogError("Can not find Unity Process symbols for Unity-iPhone phase.");
                return null;
            }

            var idLine = (string)null;
            for (var i = nameIndex - 1; i > section.Value.SectionStart; i--)
            {
                if (allLines[i].Contains(" ShellScript "))
                {
                    idLine = allLines[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(idLine))
            {
                Debug.LogError("Can not find phase ID.");
                return null;
            }

// line example: D4BF5B853AEA9FBFC39E33E9 /* ShellScript */ = {
            var parts = idLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
            {
                return null;
            }

            return parts[0];
        }

        private static (int SectionStart, int SectionEnd)? FindShellScriptSection(string[] allLines)
        {
            var firstLine = -1;
            for (var i = 0; i + 2 < allLines.Length; i++)
            {
                if (allLines[i].Contains(_targetShellScriptSectionKey))
                {
                    firstLine = i + 2;
                    break;
                }
            }

            if (firstLine <= 0)
            {
                return null;
            }

// Search for last line.
            var lastLine = -1;
            for (var i = firstLine + 1; i < allLines.Length; i++)
            {
                if (allLines[i].Contains(_targetEndShellScriptSectionKey))
                {
                    lastLine = i - 1;
                    break;
                }
            }

            return lastLine <= 0 || lastLine == firstLine ? null : (firstLine, lastLine);
        }
    }
}