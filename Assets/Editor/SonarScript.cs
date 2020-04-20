using UnityEditor;

namespace Agate.Sonarqube
{
    public static class SonarScript
    {
        public static void GenerateSln()
        {
            EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        }
    }
}