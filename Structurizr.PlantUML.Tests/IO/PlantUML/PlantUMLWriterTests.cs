using System.IO;
using System.Linq;
using Xunit;
using Structurizr.IO.PlantUML.Tests.Utilities;

namespace Structurizr.IO.PlantUML.Tests
{
    
    public class PlantUMLWriterTests
    {

        private PlantUMLWriter _plantUMLWriter;
        private Workspace _workspace;
        private StringWriter _stringWriter;

        public PlantUMLWriterTests()
        {
            _plantUMLWriter = new PlantUMLWriter();
            _workspace = new Workspace("Name", "Description");
            _stringWriter = new StringWriter();
        }

        [Fact]
        public void Test_WriteWorkspace_DoesNotThrowAnExceptionWhenPassedNullParameters()
        {
            _plantUMLWriter.Write((Workspace)null, null);
            _plantUMLWriter.Write(_workspace, null);
            _plantUMLWriter.Write((Workspace)null, _stringWriter);
        }

        [Fact]
        public void test_writeWorkspace()
        {
            PopulateWorkspace();
    
            _plantUMLWriter.Write(_workspace, _stringWriter);
            Assert.Equal(
@"@startuml
title System Landscape for Some Enterprise
component ""E-mail System"" <<Software System>> as 4
package ""Some Enterprise"" {
  actor ""User"" <<Person>> as 1
  component ""Software System"" <<Software System>> as 2
}
4 ..> 1 : Delivers e-mails to
2 ..> 4 : Sends e-mail using
1 ..> 2 : Uses
@enduml

@startuml
title Software System - System Context
component ""E-mail System"" <<Software System>> as 4
component ""Software System"" <<Software System>> as 2
actor ""User"" <<Person>> as 1
4 ..> 1 : Delivers e-mails to
2 ..> 4 : Sends e-mail using
1 ..> 2 : Uses
@enduml

@startuml
title Software System - Containers
component ""E-mail System"" <<Software System>> as 4
actor ""User"" <<Person>> as 1
package SoftwareSystem {
  component ""Database"" <<Container>> as 8
  component ""Web Application"" <<Container>> as 7
}
4 ..> 1 : Delivers e-mails to
1 ..> 7 : <<HTTP>>
7 ..> 8 : Reads from and writes to <<JDBC>>
7 ..> 4 : Sends e-mail using
@enduml

@startuml
title Software System - Web Application - Components
component ""Database"" <<Container>> as 8
component ""E-mail System"" <<Software System>> as 4
actor ""User"" <<Person>> as 1
package WebApplication {
  component ""EmailComponent"" <<Component>> as 13
  component ""SomeController"" <<Spring MVC Controller>> as 12
  component ""SomeRepository"" <<Spring Data>> as 14
}
4 ..> 1 : Delivers e-mails to
13 ..> 4 : Sends e-mails using <<SMTP>>
12 ..> 13 : Sends e-mail using
12 ..> 14 : Uses
14 ..> 8 : Reads from and writes to <<JDBC>>
1 ..> 12 : Uses <<HTTP>>
@enduml

@startuml
title Web Application - Dynamic
component ""Database"" <<Container>> as 8
component ""SomeController"" <<Spring MVC Controller>> as 12
component ""SomeRepository"" <<Spring Data>> as 14
actor ""User"" <<Person>> as 1
1 -> 12 : Requests /something
12 -> 14 : Uses
14 -> 8 : select * from something
@enduml

@startuml
title Software System - Deployment
node ""Database Server"" <<Ubuntu 12.04 LTS>> as 23 {
  node ""MySQL"" <<MySQL 5.5.x>> as 24 {
    artifact ""Database"" <<Container>> as 25
  }
}
node ""Web Server"" <<Ubuntu 12.04 LTS>> as 20 {
  node ""Apache Tomcat"" <<Apache Tomcat 8.x>> as 21 {
    artifact ""Web Application"" <<Container>> as 22
  }
}
22 ..> 25 : Reads from and writes to <<JDBC>>
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
    
        [Fact]
        public void test_writeEnterpriseContextView()
        {
            PopulateWorkspace();
    
            SystemLandscapeView systemLandscapeView = _workspace.Views.SystemLandscapeViews.First();
            _plantUMLWriter.Write(systemLandscapeView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title System Landscape for Some Enterprise
component ""E-mail System"" <<Software System>> as 4
package ""Some Enterprise"" {
  actor ""User"" <<Person>> as 1
  component ""Software System"" <<Software System>> as 2
}
4 ..> 1 : Delivers e-mails to
2 ..> 4 : Sends e-mail using
1 ..> 2 : Uses
@enduml

".UnifyNewLine(), _stringWriter.ToString());
    
        }
    
        [Fact]
        public void test_writeSystemContextView()
        {
            PopulateWorkspace();
    
            SystemContextView systemContextView = _workspace.Views.SystemContextViews.First();
            _plantUMLWriter.Write(systemContextView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title Software System - System Context
component ""E-mail System"" <<Software System>> as 4
component ""Software System"" <<Software System>> as 2
actor ""User"" <<Person>> as 1
4 ..> 1 : Delivers e-mails to
2 ..> 4 : Sends e-mail using
1 ..> 2 : Uses
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
    
        [Fact]
        public void test_writeContainerView() 
        {
            PopulateWorkspace();
    
            ContainerView containerView = _workspace.Views.ContainerViews.First();
            _plantUMLWriter.Write(containerView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title Software System - Containers
component ""E-mail System"" <<Software System>> as 4
actor ""User"" <<Person>> as 1
package SoftwareSystem {
  component ""Database"" <<Container>> as 8
  component ""Web Application"" <<Container>> as 7
}
4 ..> 1 : Delivers e-mails to
1 ..> 7 : <<HTTP>>
7 ..> 8 : Reads from and writes to <<JDBC>>
7 ..> 4 : Sends e-mail using
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
    
        [Fact]
        public void test_writeComponentsView()
        {
            PopulateWorkspace();
    
            ComponentView componentView = _workspace.Views.ComponentViews.First();
            _plantUMLWriter.Write(componentView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title Software System - Web Application - Components
component ""Database"" <<Container>> as 8
component ""E-mail System"" <<Software System>> as 4
actor ""User"" <<Person>> as 1
package WebApplication {
  component ""EmailComponent"" <<Component>> as 13
  component ""SomeController"" <<Spring MVC Controller>> as 12
  component ""SomeRepository"" <<Spring Data>> as 14
}
4 ..> 1 : Delivers e-mails to
13 ..> 4 : Sends e-mails using <<SMTP>>
12 ..> 13 : Sends e-mail using
12 ..> 14 : Uses
14 ..> 8 : Reads from and writes to <<JDBC>>
1 ..> 12 : Uses <<HTTP>>
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
    
        [Fact]
        public void test_writeDynamicView()
        {
            PopulateWorkspace();
    
            DynamicView dynamicView = _workspace.Views.DynamicViews.First();
            _plantUMLWriter.Write(dynamicView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title Web Application - Dynamic
component ""Database"" <<Container>> as 8
component ""SomeController"" <<Spring MVC Controller>> as 12
component ""SomeRepository"" <<Spring Data>> as 14
actor ""User"" <<Person>> as 1
1 -> 12 : Requests /something
12 -> 14 : Uses
14 -> 8 : select * from something
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
 
        [Fact]
        public void test_writeDeploymentView()
        {
            PopulateWorkspace();
   
            DeploymentView deploymentView = _workspace.Views.DeploymentViews.First();
            _plantUMLWriter.Write(deploymentView, _stringWriter);
    
            Assert.Equal(
@"@startuml
title Software System - Deployment
node ""Database Server"" <<Ubuntu 12.04 LTS>> as 23 {
  node ""MySQL"" <<MySQL 5.5.x>> as 24 {
    artifact ""Database"" <<Container>> as 25
  }
}
node ""Web Server"" <<Ubuntu 12.04 LTS>> as 20 {
  node ""Apache Tomcat"" <<Apache Tomcat 8.x>> as 21 {
    artifact ""Web Application"" <<Container>> as 22
  }
}
22 ..> 25 : Reads from and writes to <<JDBC>>
@enduml

".UnifyNewLine(), _stringWriter.ToString());
        }
    
        private void PopulateWorkspace()
        {
            Model model = _workspace.Model;
            ViewSet views = _workspace.Views;
            model.Enterprise = new Enterprise("Some Enterprise");
    
            Person user = model.AddPerson(Location.Internal, "User", "");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem(Location.Internal, "Software System", "");
            user.Uses(softwareSystem, "Uses");
    
            SoftwareSystem emailSystem = model.AddSoftwareSystem(Location.External, "E-mail System", "");
            softwareSystem.Uses(emailSystem, "Sends e-mail using");
            emailSystem.Delivers(user, "Delivers e-mails to");
    
            Container webApplication = softwareSystem.AddContainer("Web Application", "", "");
            Container database = softwareSystem.AddContainer("Database", "", "");
            user.Uses(webApplication, null, "HTTP");
            webApplication.Uses(database, "Reads from and writes to", "JDBC");
            webApplication.Uses(emailSystem, "Sends e-mail using");
    
            Component controller = webApplication.AddComponent("SomeController", "", "Spring MVC Controller");
            Component emailComponent = webApplication.AddComponent("EmailComponent", "");
            Component repository = webApplication.AddComponent("SomeRepository", "", "Spring Data");
            user.Uses(controller, "Uses", "HTTP");
            controller.Uses(repository, "Uses");
            controller.Uses(emailComponent, "Sends e-mail using");
            repository.Uses(database, "Reads from and writes to", "JDBC");
            emailComponent.Uses(emailSystem, "Sends e-mails using", "SMTP");

            DeploymentNode webServer = model.AddDeploymentNode("Web Server", "A server hosted at AWS EC2.", "Ubuntu 12.04 LTS");
            webServer.AddDeploymentNode("Apache Tomcat", "The live web server", "Apache Tomcat 8.x")
                    .Add(webApplication);
            DeploymentNode databaseServer = model.AddDeploymentNode("Database Server", "A server hosted at AWS EC2.", "Ubuntu 12.04 LTS");
            databaseServer.AddDeploymentNode("MySQL", "The live database server", "MySQL 5.5.x")
                    .Add(database);
    
            SystemLandscapeView
                systemLandscapeView = views.CreateSystemLandscapeView("enterpriseContext", "");
            systemLandscapeView.AddAllElements();
    
            SystemContextView systemContextView = views.CreateSystemContextView(softwareSystem, "systemContext", "");
            systemContextView.AddAllElements();
    
            ContainerView containerView = views.CreateContainerView(softwareSystem, "containers", "");
            containerView.AddAllElements();
    
            ComponentView componentView = views.CreateComponentView(webApplication, "components", "");
            componentView.AddAllElements();
    
            DynamicView dynamicView = views.CreateDynamicView(webApplication, "dynamic", "");
            dynamicView.Add(user, "Requests /something", controller);
            dynamicView.Add(controller, repository);
            dynamicView.Add(repository, "select * from something", database);

            DeploymentView deploymentView = views.CreateDeploymentView(softwareSystem, "deployment", "");
            deploymentView.AddAllDeploymentNodes();
        }
    }
}