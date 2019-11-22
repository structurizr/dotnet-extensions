using System.IO;

// Source base version copied from https://gist.github.com/coldacid/465fa8f3a4cd3fdd7b640a65ad5b86f4 (https://github.com/structurizr/dotnet/issues/47) 
// kirchsth: Extended with dynamic and deployment view
namespace Structurizr.IO.C4PlantUML
{
    public interface IPlantUMLWriter
    {
        void Write(Workspace workspace, TextWriter writer);
        void Write(View view, TextWriter writer);
    }
}