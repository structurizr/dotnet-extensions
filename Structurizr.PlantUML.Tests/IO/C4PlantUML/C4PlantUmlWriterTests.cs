using System;
using System.IO;
using System.Linq;
using Structurizr.Examples;
using Structurizr.IO.C4PlantUML.ModelExtensions;
using Structurizr.IO.PlantUML.Tests.Utilities;
using Xunit;

namespace Structurizr.IO.C4PlantUML.Tests
{
    public sealed class C4PlantUmlWriterTests : IDisposable
    {
        private C4PlantUmlWriter _plantUMLWriter;
        private Workspace _workspace;
        private StringWriter _stringWriter;

        public C4PlantUmlWriterTests()
        {
            _plantUMLWriter = new C4PlantUmlWriter();
            _workspace = new Workspace("Name", "Description");
            _stringWriter = new StringWriter();
        }

        // This call creates all BigBankPlc views, enables a simple comparison of the Structurizr created views
        [Fact]
        public void test_writeBigBankPlcWorkspace()
        {
            // the BigBankPlc workspace is extended with some Rel_xxxx layout tags, that the UI results can be simpler compared
            _workspace = BigBankPlc.Create();
            AddLayoutDetails(_workspace);

/*
            using (var writer = new StringWriter())
            {
                new Structurizr.IO.Json.JsonWriter(true).Write(_workspace, writer);
                var json = writer.GetStringBuilder().ToString();
                json = json;
            }
*/

            _plantUMLWriter.Write(_workspace, _stringWriter);
            Assert.Equal(
@"@startuml
!include <C4/C4_Context>

' Structurizr.SystemLandscapeView: SystemLandscape
title System Landscape for Big Bank plc

LAYOUT_WITH_LEGEND()

Person_Ext(PersonalBankingCustomer__9bc576, ""Personal Banking Customer"", ""A customer of the bank, with personal bank accounts."")
Enterprise_Boundary(BigBankplc, ""Big Bank plc"") {
  Person(BackOfficeStaff__5f761d, ""Back Office Staff"", ""Administration and support staff within the bank."")
  Person(CustomerServiceStaff__a35be5, ""Customer Service Staff"", ""Customer service staff within the bank."")
  System(ATM__22fc739, ""ATM"", ""Allows customers to withdraw cash."")
  System(EmailSystem__2908eb9, ""E-mail System"", ""The internal Microsoft Exchange e-mail system."")
  System(InternetBankingSystem__2aef74c, ""Internet Banking System"", ""Allows customers to view information about their bank accounts, and make payments."")
  System(MainframeBankingSystem__f50ffa, ""Mainframe Banking System"", ""Stores all of the core banking information about customers, accounts, transactions, etc."")
}
Rel_Down(ATM__22fc739, MainframeBankingSystem__f50ffa, ""Uses"")
Rel_Down(BackOfficeStaff__5f761d, MainframeBankingSystem__f50ffa, ""Uses"")
Rel_Down(CustomerServiceStaff__a35be5, MainframeBankingSystem__f50ffa, ""Uses"")
Rel_Up(EmailSystem__2908eb9, PersonalBankingCustomer__9bc576, ""Sends e-mails to"")
Rel_Down(InternetBankingSystem__2aef74c, EmailSystem__2908eb9, ""Sends e-mail using"")
Rel_Down(InternetBankingSystem__2aef74c, MainframeBankingSystem__f50ffa, ""Uses"")
Rel(PersonalBankingCustomer__9bc576, ATM__22fc739, ""Withdraws cash using"")
Rel(PersonalBankingCustomer__9bc576, CustomerServiceStaff__a35be5, ""Asks questions to"", ""Telephone"")
Rel(PersonalBankingCustomer__9bc576, InternetBankingSystem__2aef74c, ""Uses"")
@enduml

@startuml
!include <C4/C4_Context>

' Structurizr.SystemContextView: SystemContext
title Internet Banking System - System Context

LAYOUT_WITH_LEGEND()

System(EmailSystem__2908eb9, ""E-mail System"", ""The internal Microsoft Exchange e-mail system."")
System(InternetBankingSystem__2aef74c, ""Internet Banking System"", ""Allows customers to view information about their bank accounts, and make payments."")
System(MainframeBankingSystem__f50ffa, ""Mainframe Banking System"", ""Stores all of the core banking information about customers, accounts, transactions, etc."")
Person_Ext(PersonalBankingCustomer__9bc576, ""Personal Banking Customer"", ""A customer of the bank, with personal bank accounts."")
Rel_Up(EmailSystem__2908eb9, PersonalBankingCustomer__9bc576, ""Sends e-mails to"")
Rel_Right(InternetBankingSystem__2aef74c, EmailSystem__2908eb9, ""Sends e-mail using"")
Rel(InternetBankingSystem__2aef74c, MainframeBankingSystem__f50ffa, ""Uses"")
Rel(PersonalBankingCustomer__9bc576, InternetBankingSystem__2aef74c, ""Uses"")
@enduml

@startuml
!include <C4/C4_Container>

' Structurizr.ContainerView: Containers
title Internet Banking System - Containers

LAYOUT_WITH_LEGEND()

System(EmailSystem__2908eb9, ""E-mail System"", ""The internal Microsoft Exchange e-mail system."")
System(MainframeBankingSystem__f50ffa, ""Mainframe Banking System"", ""Stores all of the core banking information about customers, accounts, transactions, etc."")
Person_Ext(PersonalBankingCustomer__9bc576, ""Personal Banking Customer"", ""A customer of the bank, with personal bank accounts."")
System_Boundary(InternetBankingSystem__2aef74c, ""Internet Banking System"") {
  Container(InternetBankingSystem__APIApplication__2c36bed, ""API Application"", ""Java and Spring MVC"", ""Provides Internet banking functionality via a JSON/HTTPS API."")
  ContainerDb(InternetBankingSystem__Database__18307f7, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
  Container(InternetBankingSystem__MobileApp__38a070b, ""Mobile App"", ""Xamarin"", ""Provides a limited subset of the Internet banking functionality to customers via their mobile device."")
  Container(InternetBankingSystem__SinglePageApplication__1414c79, ""Single-Page Application"", ""JavaScript and Angular"", ""Provides all of the Internet banking functionality to customers via their web browser."")
  Container(InternetBankingSystem__WebApplication__1bb919c, ""Web Application"", ""Java and Spring MVC"", ""Delivers the static content and the Internet banking single page application."")
}
Rel_Left(InternetBankingSystem__APIApplication__2c36bed, InternetBankingSystem__Database__18307f7, ""Reads from and writes to"", ""JDBC"")
Rel_Up(InternetBankingSystem__APIApplication__2c36bed, EmailSystem__2908eb9, ""Sends e-mail using"", ""SMTP"")
Rel_Left(InternetBankingSystem__APIApplication__2c36bed, MainframeBankingSystem__f50ffa, ""Uses"", ""XML/HTTPS"")
Rel_Up(EmailSystem__2908eb9, PersonalBankingCustomer__9bc576, ""Sends e-mails to"")
Rel(InternetBankingSystem__MobileApp__38a070b, InternetBankingSystem__APIApplication__2c36bed, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(PersonalBankingCustomer__9bc576, InternetBankingSystem__MobileApp__38a070b, ""Uses"")
Rel(PersonalBankingCustomer__9bc576, InternetBankingSystem__SinglePageApplication__1414c79, ""Uses"")
Rel(PersonalBankingCustomer__9bc576, InternetBankingSystem__WebApplication__1bb919c, ""Uses"", ""HTTPS"")
Rel(InternetBankingSystem__SinglePageApplication__1414c79, InternetBankingSystem__APIApplication__2c36bed, ""Makes API calls to"", ""JSON/HTTPS"")
Rel_Right(InternetBankingSystem__WebApplication__1bb919c, InternetBankingSystem__SinglePageApplication__1414c79, ""Delivers to the customer's web browser"")
@enduml

@startuml
!include <C4/C4_Component>

' Structurizr.ComponentView: Components
title Internet Banking System - API Application - Components

LAYOUT_WITH_LEGEND()

ContainerDb(InternetBankingSystem__Database__18307f7, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
System(EmailSystem__2908eb9, ""E-mail System"", ""The internal Microsoft Exchange e-mail system."")
System(MainframeBankingSystem__f50ffa, ""Mainframe Banking System"", ""Stores all of the core banking information about customers, accounts, transactions, etc."")
Container(InternetBankingSystem__MobileApp__38a070b, ""Mobile App"", ""Xamarin"", ""Provides a limited subset of the Internet banking functionality to customers via their mobile device."")
Container(InternetBankingSystem__SinglePageApplication__1414c79, ""Single-Page Application"", ""JavaScript and Angular"", ""Provides all of the Internet banking functionality to customers via their web browser."")
Container_Boundary(InternetBankingSystem__APIApplication__2c36bed, ""API Application"") {
  Component(InternetBankingSystem__APIApplication__AccountsSummaryController__3f81fb2, ""Accounts Summary Controller"", ""Spring MVC Rest Controller"", ""Provides customers with a summary of their bank accounts."")
  Component(InternetBankingSystem__APIApplication__EmailComponent__24ec565, ""E-mail Component"", ""Spring Bean"", ""Sends e-mails to users."")
  Component(InternetBankingSystem__APIApplication__MainframeBankingSystemFacade__2493dd9, ""Mainframe Banking System Facade"", ""Spring Bean"", ""A facade onto the mainframe banking system."")
  Component(InternetBankingSystem__APIApplication__ResetPasswordController__23f0eac, ""Reset Password Controller"", ""Spring MVC Rest Controller"", ""Allows users to reset their passwords with a single use URL."")
  Component(InternetBankingSystem__APIApplication__SecurityComponent__a4474, ""Security Component"", ""Spring Bean"", ""Provides functionality related to signing in, changing passwords, etc."")
  Component(InternetBankingSystem__APIApplication__SignInController__22cc62b, ""Sign In Controller"", ""Spring MVC Rest Controller"", ""Allows users to sign in to the Internet Banking System."")
}
Rel(InternetBankingSystem__APIApplication__AccountsSummaryController__3f81fb2, InternetBankingSystem__APIApplication__MainframeBankingSystemFacade__2493dd9, ""Uses"")
Rel(InternetBankingSystem__APIApplication__EmailComponent__24ec565, EmailSystem__2908eb9, ""Sends e-mail using"")
Rel(InternetBankingSystem__APIApplication__MainframeBankingSystemFacade__2493dd9, MainframeBankingSystem__f50ffa, ""Uses"", ""XML/HTTPS"")
Rel(InternetBankingSystem__MobileApp__38a070b, InternetBankingSystem__APIApplication__AccountsSummaryController__3f81fb2, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__MobileApp__38a070b, InternetBankingSystem__APIApplication__ResetPasswordController__23f0eac, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__MobileApp__38a070b, InternetBankingSystem__APIApplication__SignInController__22cc62b, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__APIApplication__ResetPasswordController__23f0eac, InternetBankingSystem__APIApplication__EmailComponent__24ec565, ""Uses"")
Rel(InternetBankingSystem__APIApplication__ResetPasswordController__23f0eac, InternetBankingSystem__APIApplication__SecurityComponent__a4474, ""Uses"")
Rel(InternetBankingSystem__APIApplication__SecurityComponent__a4474, InternetBankingSystem__Database__18307f7, ""Reads from and writes to"", ""JDBC"")
Rel(InternetBankingSystem__APIApplication__SignInController__22cc62b, InternetBankingSystem__APIApplication__SecurityComponent__a4474, ""Uses"")
Rel(InternetBankingSystem__SinglePageApplication__1414c79, InternetBankingSystem__APIApplication__AccountsSummaryController__3f81fb2, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__SinglePageApplication__1414c79, InternetBankingSystem__APIApplication__ResetPasswordController__23f0eac, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__SinglePageApplication__1414c79, InternetBankingSystem__APIApplication__SignInController__22cc62b, ""Makes API calls to"", ""JSON/HTTPS"")
@enduml

@startuml
!include <C4/C4_Component>
' C4_Dynamic.puml is missing, simulate it with following definitions
' Scope: Interactions in an enterprise, software system or container.
' Primary and supporting elements: Depends on the diagram scope - 
'     enterprise - people and software systems related to the enterprise in scope 
'     software system - see system context or container diagrams, 
'     container - see component diagram.
' Intended audience: Technical and non-technical people, inside and outside of the software development team.

' Dynamic diagram introduces (automatically) numbered interactions: 
'     Interact(): used automatic calculated index, 
'     Interact2(): index can be explicit defined,
'     SetIndex(): set the next index, 
'     GetIndex(): get the index and automatically increase index

' Index
' ##################################

!function $inc_($value, $step=1)
  !return $value + $step
!endfunction

!$index=1

!function SetIndex($new_index)
  !$index=$new_index
!endfunction

!function GetIndex($auto_increase=1)
  !$old = $index
  !$index=$inc_($index, $auto_increase)
  !return $old
!endfunction

' Interact
' ##################################
!define Interact2(e_index, e_from, e_to, e_label) Rel(e_from, e_to, ""e_index: e_label"")
!define Interact2(e_index, e_from, e_to, e_label, e_techn) Rel(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Back(e_index, e_from, e_to, e_label) Rel_Back(e_from, e_to, ""e_index: e_label"")
!define Interact2_Back(e_index, e_from, e_to, e_label, e_techn) Rel_Back(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Neighbor(e_index, e_from, e_to, e_label) Rel_Neighbor(e_from, e_to, ""e_index: e_label"")
!define Interact2_Neighbor(e_index, e_from, e_to, e_label, e_techn) Rel_Neighbor(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Back_Neighbor(e_index, e_from, e_to, e_label) Rel_Back_Neighbor(e_from, e_to, ""e_index: e_label"")
!define Interact2_Back_Neighbor(e_index, e_from, e_to, e_label, e_techn) Rel_Back_Neighbor(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_D(e_index, e_from, e_to, e_label) Rel_D(e_from, e_to, ""e_index: e_label"")
!define Interact2_D(e_index, e_from, e_to, e_label, e_techn) Rel_D(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Down(e_index, e_from, e_to, e_label) Rel_Down(e_from, e_to, ""e_index: e_label"")
!define Interact2_Down(e_index, e_from, e_to, e_label, e_techn) Rel_Down(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_U(e_index, e_from, e_to, e_label) Rel_U(e_from, e_to, ""e_index: e_label"")
!define Interact2_U(e_index, e_from, e_to, e_label, e_techn) Rel_U(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Up(e_index, e_from, e_to, e_label) Rel_Up(e_from, e_to, ""e_index: e_label"")
!define Interact2_Up(e_index, e_from, e_to, e_label, e_techn) Rel_Up(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_L(e_index, e_from, e_to, e_label) Rel_L(e_from, e_to, ""e_index: e_label"")
!define Interact2_L(e_index, e_from, e_to, e_label, e_techn) Rel_L(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Left(e_index, e_from, e_to, e_label) Rel_Left(e_from, e_to, ""e_index: e_label"")
!define Interact2_Left(e_index, e_from, e_to, e_label, e_techn) Rel_Left(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_R(e_index, e_from, e_to, e_label) Rel_R(e_from, e_to, ""e_index: e_label"")
!define Interact2_R(e_index, e_from, e_to, e_label, e_techn) Rel_R(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Right(e_index, e_from, e_to, e_label) Rel_Right(e_from, e_to, ""e_index: e_label"")
!define Interact2_Right(e_index, e_from, e_to, e_label, e_techn) Rel_Right(e_from, e_to, ""e_index: e_label"", e_techn)

!unquoted function Interact($e_from, $e_to, $e_label) 
  Interact2($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact($e_from, $e_to, $e_label, $e_techn) 
  Interact2($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Back($e_from, $e_to, $e_label) 
  Interact2_Back($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Back($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Back($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Neighbor($e_from, $e_to, $e_label) 
  Interact2_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Neighbor($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Back_Neighbor($e_from, $e_to, $e_label) 
  Interact2_Back_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Back_Neighbor($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Back_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_D($e_from, $e_to, $e_label) 
  Interact2_D($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_D($e_from, $e_to, $e_label, $e_techn) 
  Interact2_D($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Down($e_from, $e_to, $e_label) 
  Interact2_Down($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Down($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Down($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_U($e_from, $e_to, $e_label) 
  Interact2_U($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_U($e_from, $e_to, $e_label, $e_techn) 
  Interact2_U($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Up($e_from, $e_to, $e_label) 
  Interact2_Up($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Up($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Up($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_L($e_from, $e_to, $e_label) 
  Interact2_L($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_L($e_from, $e_to, $e_label, $e_techn) 
  Interact2_L($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Left($e_from, $e_to, $e_label) 
  Interact2_Left($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Left($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Left($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_R($e_from, $e_to, $e_label) 
  Interact2_R($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_R($e_from, $e_to, $e_label, $e_techn) 
  Interact2_R($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Right($e_from, $e_to, $e_label) 
  Interact2_Right($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Right($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Right($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

' Structurizr.DynamicView: SignIn
title API Application - Dynamic

LAYOUT_WITH_LEGEND()

ContainerDb(InternetBankingSystem__Database__18307f7, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
Container(InternetBankingSystem__SinglePageApplication__1414c79, ""Single-Page Application"", ""JavaScript and Angular"", ""Provides all of the Internet banking functionality to customers via their web browser."")
Container_Boundary(InternetBankingSystem__APIApplication__2c36bed, ""API Application"") {
  Component(InternetBankingSystem__APIApplication__SecurityComponent__a4474, ""Security Component"", ""Spring Bean"", ""Provides functionality related to signing in, changing passwords, etc."")
  Component(InternetBankingSystem__APIApplication__SignInController__22cc62b, ""Sign In Controller"", ""Spring MVC Rest Controller"", ""Allows users to sign in to the Internet Banking System."")
}
Interact2_Right(""1"", InternetBankingSystem__SinglePageApplication__1414c79, InternetBankingSystem__APIApplication__SignInController__22cc62b, ""Submits credentials to"", ""JSON/HTTPS"")
Interact2(""2"", InternetBankingSystem__APIApplication__SignInController__22cc62b, InternetBankingSystem__APIApplication__SecurityComponent__a4474, ""Calls isAuthenticated() on"")
Interact2_Right(""3"", InternetBankingSystem__APIApplication__SecurityComponent__a4474, InternetBankingSystem__Database__18307f7, ""select * from users where username = ?"", ""JDBC"")
@enduml

@startuml
!include <C4/C4_Container>
' C4_Deployment.puml is missing, simulate it with following definitions
' Scope: A single software system.
' Primary elements: Deployment nodes and containers within the software system in scope.
' Intended audience: Technical people inside and outside of the software development team; including software architects, developers and operations/support staff.

' Colors
' ##################################
!define NODE_FONT_COLOR    #444444
!define NODE_BG_COLOR      #FFFFFF

' Styling
' ##################################

skinparam rectangle<<node>> {
  Shadowing false
  StereotypeFontSize 0
  FontColor NODE_FONT_COLOR
  BackgroundColor NODE_BG_COLOR
  BorderColor #444444
}

' Layout
' ##################################

!definelong LAYOUT_WITH_LEGEND
hide stereotype
legend right
|=                    |= Type                |
|<NODE_BG_COLOR>      | deployment node      |
|<CONTAINER_BG_COLOR> | deployment container |
endlegend
!enddefinelong

' Nodes
' ##################################
' PlantUML does not support automatic line breaks of container, if e_techn is very long insert line breaks with 
' ""</size>\n<size:TECHN_FONT_SIZE>""
!define Node(e_alias, e_label, e_techn) rectangle ""==e_label\n<size:TECHN_FONT_SIZE>[e_techn]</size>"" <<node>> as e_alias

' Structurizr.DeploymentView: DevelopmentDeployment
title Internet Banking System - Deployment

LAYOUT_WITH_LEGEND()

Node(Deployment__Development__DeveloperLaptop__389f399, ""Developer Laptop"", ""Microsoft Windows 10 or Apple </size>\n<size:TECHN_FONT_SIZE>macOS"") {
  Node(Deployment__Development__DeveloperLaptop__DockerContainerWebServer__1b73d2e, ""Docker Container - Web Server"", ""Docker"") {
    Node(Deployment__Development__DeveloperLaptop__DockerContainerWebServer__ApacheTomcat__1cc9f55, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
      Container(InternetBankingSystem__WebApplication1__28f79f6, ""Web Application"", ""Java and Spring MVC"", ""Delivers the static content and the Internet banking single page application."")
      Container(InternetBankingSystem__APIApplication1__1f227f4, ""API Application"", ""Java and Spring MVC"", ""Provides Internet banking functionality via a JSON/HTTPS API."")
    }
  }
  Node(Deployment__Development__DeveloperLaptop__DockerContainerDatabaseServer__2eae566, ""Docker Container - Database Server"", ""Docker"") {
    Node(Deployment__Development__DeveloperLaptop__DockerContainerDatabaseServer__DatabaseServer__24d13de, ""Database Server"", ""Oracle 12c"") {
      ContainerDb(InternetBankingSystem__Database1__3296ca6, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
    }
  }
  Node(Deployment__Development__DeveloperLaptop__WebBrowser__3930fd, ""Web Browser"", ""Google Chrome, Mozilla </size>\n<size:TECHN_FONT_SIZE>Firefox, Apple Safari or </size>\n<size:TECHN_FONT_SIZE>Microsoft Edge"") {
    Container(InternetBankingSystem__SinglePageApplication1__bbe85d, ""Single-Page Application"", ""JavaScript and Angular"", ""Provides all of the Internet banking functionality to customers via their web browser."")
  }
}
Rel(InternetBankingSystem__APIApplication1__1f227f4, InternetBankingSystem__Database1__3296ca6, ""Reads from and writes to"", ""JDBC"")
Rel(InternetBankingSystem__SinglePageApplication1__bbe85d, InternetBankingSystem__APIApplication1__1f227f4, ""Makes API calls to"", ""JSON/HTTPS"")
Rel_Up(InternetBankingSystem__WebApplication1__28f79f6, InternetBankingSystem__SinglePageApplication1__bbe85d, ""Delivers to the customer's web browser"")
@enduml

@startuml
!include <C4/C4_Container>
' C4_Deployment.puml is missing, simulate it with following definitions
' Scope: A single software system.
' Primary elements: Deployment nodes and containers within the software system in scope.
' Intended audience: Technical people inside and outside of the software development team; including software architects, developers and operations/support staff.

' Colors
' ##################################
!define NODE_FONT_COLOR    #444444
!define NODE_BG_COLOR      #FFFFFF

' Styling
' ##################################

skinparam rectangle<<node>> {
  Shadowing false
  StereotypeFontSize 0
  FontColor NODE_FONT_COLOR
  BackgroundColor NODE_BG_COLOR
  BorderColor #444444
}

' Layout
' ##################################

!definelong LAYOUT_WITH_LEGEND
hide stereotype
legend right
|=                    |= Type                |
|<NODE_BG_COLOR>      | deployment node      |
|<CONTAINER_BG_COLOR> | deployment container |
endlegend
!enddefinelong

' Nodes
' ##################################
' PlantUML does not support automatic line breaks of container, if e_techn is very long insert line breaks with 
' ""</size>\n<size:TECHN_FONT_SIZE>""
!define Node(e_alias, e_label, e_techn) rectangle ""==e_label\n<size:TECHN_FONT_SIZE>[e_techn]</size>"" <<node>> as e_alias

' Structurizr.DeploymentView: LiveDeployment
title Internet Banking System - Deployment

LAYOUT_WITH_LEGEND()

Node(Deployment__Live__BigBankplc__3ffe15e, ""Big Bank plc"", ""Big Bank plc data center"") {
  Node(Deployment__Live__BigBankplc__bigbankweb***__3f92e18, ""bigbank-web*** (x4)"", ""Ubuntu 16.04 LTS"") {
    Node(Deployment__Live__BigBankplc__bigbankweb***__ApacheTomcat__27b4383, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
      Container(InternetBankingSystem__WebApplication2__1720850, ""Web Application"", ""Java and Spring MVC"", ""Delivers the static content and the Internet banking single page application."")
    }
  }
  Node(Deployment__Live__BigBankplc__bigbankapi***__263d9e8, ""bigbank-api*** (x8)"", ""Ubuntu 16.04 LTS"") {
    Node(Deployment__Live__BigBankplc__bigbankapi***__ApacheTomcat__3b84ab, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
      Container(InternetBankingSystem__APIApplication2__1408a33, ""API Application"", ""Java and Spring MVC"", ""Provides Internet banking functionality via a JSON/HTTPS API."")
    }
  }
  Node(Deployment__Live__BigBankplc__bigbankdb01__35ec592, ""bigbank-db01"", ""Ubuntu 16.04 LTS"") {
    Node(Deployment__Live__BigBankplc__bigbankdb01__OraclePrimary__19fd8f, ""Oracle - Primary"", ""Oracle 12c"") {
      ContainerDb(InternetBankingSystem__Database2__1c974ec, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
    }
  }
  Node(Deployment__Live__BigBankplc__bigbankdb02__1db08a2, ""bigbank-db02"", ""Ubuntu 16.04 LTS"") {
    Node(Deployment__Live__BigBankplc__bigbankdb02__OracleSecondary__1c4ec22, ""Oracle - Secondary"", ""Oracle 12c"") {
      ContainerDb(InternetBankingSystem__Database3__d89394, ""Database"", ""Relational Database Schema"", ""Stores user registration information, hashed authentication credentials, access logs, etc."")
    }
  }
}
Node(Deployment__Live__Customer'scomputer__2510bf3, ""Customer's computer"", ""Microsoft Windows or Apple </size>\n<size:TECHN_FONT_SIZE>macOS"") {
  Node(Deployment__Live__Customer'scomputer__WebBrowser__ba951, ""Web Browser"", ""Google Chrome, Mozilla </size>\n<size:TECHN_FONT_SIZE>Firefox, Apple Safari or </size>\n<size:TECHN_FONT_SIZE>Microsoft Edge"") {
    Container(InternetBankingSystem__SinglePageApplication2__298b31c, ""Single-Page Application"", ""JavaScript and Angular"", ""Provides all of the Internet banking functionality to customers via their web browser."")
  }
}
Node(Deployment__Live__Customer'smobiledevice__1d6bcb6, ""Customer's mobile device"", ""Apple iOS or Android"") {
  Container(InternetBankingSystem__MobileApp1__d004b3, ""Mobile App"", ""Xamarin"", ""Provides a limited subset of the Internet banking functionality to customers via their mobile device."")
}
Rel(InternetBankingSystem__APIApplication2__1408a33, InternetBankingSystem__Database2__1c974ec, ""Reads from and writes to"", ""JDBC"")
Rel(InternetBankingSystem__APIApplication2__1408a33, InternetBankingSystem__Database3__d89394, ""Reads from and writes to"", ""JDBC"")
Rel(InternetBankingSystem__MobileApp1__d004b3, InternetBankingSystem__APIApplication2__1408a33, ""Makes API calls to"", ""JSON/HTTPS"")
Rel(InternetBankingSystem__SinglePageApplication2__298b31c, InternetBankingSystem__APIApplication2__1408a33, ""Makes API calls to"", ""JSON/HTTPS"")
Rel_Up(InternetBankingSystem__WebApplication2__1720850, InternetBankingSystem__SinglePageApplication2__298b31c, ""Delivers to the customer's web browser"")
Rel_Left(Deployment__Live__BigBankplc__bigbankdb01__OraclePrimary__19fd8f, Deployment__Live__BigBankplc__bigbankdb02__OracleSecondary__1c4ec22, ""Replicates data to"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        /// <summary>
        /// Add some layout details, enables a simpler compare with the original files
        /// </summary>
        private void AddLayoutDetails(Workspace workspace)
        {
            // Define some common layout tags to the model relations (via AddTags)
            // all SystemLandscapeView, SystemContext, ... (update relation): 
            //     Rel_Up(EmailSystem, PersonalBankingCustomer, ""Sends e-mails to"")
            //     Rel_Right(InternetBankingSystem, EmailSystem, ""Sends e-mail using"")
            var emailSystem = workspace.Model.GetElementWithCanonicalName("/E-mail System");
            var personalBankingCustomer = workspace.Model.GetElementWithCanonicalName("/Personal Banking Customer");
            var internetBankingSystem = workspace.Model.GetElementWithCanonicalName("/Internet Banking System");

            var systemLandscapeView = workspace.Views.SystemLandscapeViews.First();
            systemLandscapeView.Relationships
                .Select(rv => rv.Relationship)
                .First(r => r.SourceId == emailSystem.Id && r.DestinationId == personalBankingCustomer.Id)
                .SetDirection(DirectionValues.Up);
            systemLandscapeView.Relationships
                .Select(rv => rv.Relationship)
                .First(r => r.SourceId == internetBankingSystem.Id && r.DestinationId == emailSystem.Id)
                .SetDirection(DirectionValues.Right);

            // but only SystemLandscapeView should use Down relations, therefore add the tags relation view specific (via AddViewTags)
            var mainframeBankingSystem = workspace.Model.GetElementWithCanonicalName("/Mainframe Banking System");
            foreach (var relationshipView in systemLandscapeView.Relationships
                .Where(rv => rv.Relationship.DestinationId == mainframeBankingSystem.Id))
            {
                relationshipView.SetDirection(DirectionValues.Down);
            }

            // and overwrite the "to generic specified" relation tag "Rel_Right(..." with the view specific "Rel_Down(..." tag
            // Rel_Down(InternetBankingSystem, EmailSystem, "Sends e-mail using")
            systemLandscapeView.Relationships
                .First(rv => rv.Relationship.SourceId == internetBankingSystem.Id &&
                             rv.Relationship.DestinationId == emailSystem.Id)
                .SetDirection(DirectionValues.Down);

            // DynamicView
            //     Rel_Right(InternetBankingSystem__SinglePageApplication, InternetBankingSystem__APIApplication__SignInController, ...)
            //     Rel_Right(InternetBankingSystem__APIApplication__SecurityComponent, InternetBankingSystem__Database, ...)
            var singlePageApplication =
                workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Single-Page Application");
            var signInController =
                workspace.Model.GetElementWithCanonicalName("/Internet Banking System/API Application/Sign In Controller");
            var securityComponent =
                workspace.Model.GetElementWithCanonicalName("/Internet Banking System/API Application/Security Component");
            var database = workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Database") as Container;
            database.SetIsDatabase(true);
            var dynamicView = workspace.Views.DynamicViews.First();
            dynamicView.Relationships
                .First(r => r.Relationship.SourceId == singlePageApplication.Id &&
                            r.Relationship.DestinationId == signInController.Id)
                .SetDirection(DirectionValues.Right);
            dynamicView.Relationships
                .First(r => r.Relationship.SourceId == securityComponent.Id && r.Relationship.DestinationId == database.Id)
                .SetDirection(DirectionValues.Right);

            // ContainerView
            //     Rel_Up(InternetBankingSystem__WebApplication, InternetBankingSystem__SinglePageApplication, "Delivers to the customer's web browser")
            var apiApplication = workspace.Model.GetElementWithCanonicalName("/Internet Banking System/API Application");
            var webApplication = workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Web Application");


            var containerView = workspace.Views.ContainerViews.First();
            containerView.Relationships
                .First(r => r.Relationship.SourceId == apiApplication.Id &&
                            r.Relationship.DestinationId == database.Id)
                .SetDirection(DirectionValues.Left);
            containerView.Relationships
                .First(r => r.Relationship.SourceId == apiApplication.Id &&
                            r.Relationship.DestinationId == emailSystem.Id)
                .SetDirection(DirectionValues.Up);
            containerView.Relationships
                .First(r => r.Relationship.SourceId == apiApplication.Id &&
                            r.Relationship.DestinationId == mainframeBankingSystem.Id)
                .SetDirection(DirectionValues.Left);
            containerView.Relationships
                .First(r => r.Relationship.SourceId == emailSystem.Id &&
                            r.Relationship.DestinationId == personalBankingCustomer.Id)
                .SetDirection(DirectionValues.Up);
            containerView.Relationships
                .First(r => r.Relationship.SourceId == webApplication.Id &&
                            r.Relationship.DestinationId == singlePageApplication.Id)
                .SetDirection(DirectionValues.Right);

            // DeploymentView´(with already copied relations): DevelopmentDeployment, LiveDeployment
            //     Rel_Up(InternetBankingSystem__WebApplication1, InternetBankingSystem__SinglePageApplication1, "Delivers to the customer's web browser") 
            //     Rel_Up(InternetBankingSystem__WebApplication2, InternetBankingSystem__SinglePageApplication2, "Delivers to the customer's web browser")
            //     Rel_Left(Deployment__Live__BigBankplc__bigbankdb01__OraclePrimary, Deployment__Live__BigBankplc__bigbankdb02__OracleSecondary, "Replicates data to")
            var webApplication1 = workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Web Application[1]");
            var webApplication2 = workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Web Application[2]");
            var singlePageApplication1 =
                workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Single-Page Application[1]");
            var singlePageApplication2 =
                workspace.Model.GetElementWithCanonicalName("/Internet Banking System/Single-Page Application[2]");
            var oraclePrimary =
                workspace.Model.GetElementWithCanonicalName("/Deployment/Live/Big Bank plc/bigbank-db01/Oracle - Primary");
            var oracleSecondary =
                workspace.Model.GetElementWithCanonicalName("/Deployment/Live/Big Bank plc/bigbank-db02/Oracle - Secondary");
            var developmentDeploymentView = workspace.Views.DeploymentViews.First();
            var liveDeploymentView = workspace.Views.DeploymentViews.Last();
            developmentDeploymentView.Relationships
                .First(r => r.Relationship.SourceId == webApplication1.Id &&
                            r.Relationship.DestinationId == singlePageApplication1.Id)
                .SetDirection(DirectionValues.Up);
            liveDeploymentView.Relationships
                .First(r => r.Relationship.SourceId == webApplication2.Id &&
                            r.Relationship.DestinationId == singlePageApplication2.Id)
                .SetDirection(DirectionValues.Up);
            liveDeploymentView.Relationships
                .First(r => r.Relationship.SourceId == oraclePrimary.Id && r.Relationship.DestinationId == oracleSecondary.Id)
                .SetDirection(DirectionValues.Left);
        }

        [Fact]
        public void test_writeWorkspace_WithCustomBaseUrl()
        {
            PopulateWorkspace();

            _plantUMLWriter.CustomBaseUrl = @"https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/";
            _plantUMLWriter.Write(_workspace, _stringWriter);
            Assert.Equal(
@"@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Context.puml

' Structurizr.SystemLandscapeView: enterpriseContext
title System Landscape for Some Enterprise

LAYOUT_WITH_LEGEND()

System_Ext(EmailSystem__1127701, ""E-mail System"")
Enterprise_Boundary(SomeEnterprise, ""Some Enterprise"") {
  Person(User__387cc75, ""User"")
  System(SoftwareSystem__31d545b, ""Software System"")
}
Rel(EmailSystem__1127701, User__387cc75, ""Delivers e-mails to"")
Rel(SoftwareSystem__31d545b, EmailSystem__1127701, ""Sends e-mail using"")
Rel(User__387cc75, SoftwareSystem__31d545b, ""Uses"")
@enduml

@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Context.puml

' Structurizr.SystemContextView: systemContext
title Software System - System Context

LAYOUT_WITH_LEGEND()

Enterprise_Boundary(SomeEnterprise, ""Some Enterprise"") {
  System_Ext(EmailSystem__1127701, ""E-mail System"")
  System(SoftwareSystem__31d545b, ""Software System"")
  Person(User__387cc75, ""User"")
Rel(EmailSystem__1127701, User__387cc75, ""Delivers e-mails to"")
Rel(SoftwareSystem__31d545b, EmailSystem__1127701, ""Sends e-mail using"")
Rel(User__387cc75, SoftwareSystem__31d545b, ""Uses"")
}
@enduml

@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Container.puml

' Structurizr.ContainerView: containers
title Software System - Containers

LAYOUT_WITH_LEGEND()

System_Ext(EmailSystem__1127701, ""E-mail System"")
Person(User__387cc75, ""User"")
System_Boundary(SoftwareSystem__31d545b, ""Software System"") {
  ContainerDb(SoftwareSystem__Database__39bccb8, ""Database"", ""Relational Database Schema"", ""Stores information"")
  Container(SoftwareSystem__WebApplication__d2a342, ""Web Application"", ""Java and spring MVC"", ""Delivers content"")
}
Rel(EmailSystem__1127701, User__387cc75, ""Delivers e-mails to"")
Rel(User__387cc75, SoftwareSystem__WebApplication__d2a342, """", ""HTTP"")
Rel(SoftwareSystem__WebApplication__d2a342, SoftwareSystem__Database__39bccb8, ""Reads from and writes to"", ""JDBC"")
Rel(SoftwareSystem__WebApplication__d2a342, EmailSystem__1127701, ""Sends e-mail using"")
@enduml

@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Component.puml

' Structurizr.ComponentView: components
title Software System - Web Application - Components

LAYOUT_WITH_LEGEND()

ContainerDb(SoftwareSystem__Database__39bccb8, ""Database"", ""Relational Database Schema"", ""Stores information"")
System_Ext(EmailSystem__1127701, ""E-mail System"")
Person(User__387cc75, ""User"")
Container_Boundary(SoftwareSystem__WebApplication__d2a342, ""Web Application"") {
  Component(SoftwareSystem__WebApplication__EmailComponent__895000, ""EmailComponent"", """")
  Component(SoftwareSystem__WebApplication__SomeController__341621c, ""SomeController"", ""Spring MVC Controller"")
  Component(SoftwareSystem__WebApplication__SomeRepository__6d9009, ""SomeRepository"", ""Spring Data"")
}
Rel(EmailSystem__1127701, User__387cc75, ""Delivers e-mails to"")
Rel(SoftwareSystem__WebApplication__EmailComponent__895000, EmailSystem__1127701, ""Sends e-mails using"", ""SMTP"")
Rel(SoftwareSystem__WebApplication__SomeController__341621c, SoftwareSystem__WebApplication__EmailComponent__895000, ""Sends e-mail using"")
Rel(SoftwareSystem__WebApplication__SomeController__341621c, SoftwareSystem__WebApplication__SomeRepository__6d9009, ""Uses"")
Rel(SoftwareSystem__WebApplication__SomeRepository__6d9009, SoftwareSystem__Database__39bccb8, ""Reads from and writes to"", ""JDBC"")
Rel(User__387cc75, SoftwareSystem__WebApplication__SomeController__341621c, ""Uses"", ""HTTP"")
@enduml

@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Dynamic.puml

' Structurizr.DynamicView: dynamic
title Web Application - Dynamic

LAYOUT_WITH_LEGEND()

ContainerDb(SoftwareSystem__Database__39bccb8, ""Database"", ""Relational Database Schema"", ""Stores information"")
Person(User__387cc75, ""User"")
Container_Boundary(SoftwareSystem__WebApplication__d2a342, ""Web Application"") {
  Component(SoftwareSystem__WebApplication__SomeController__341621c, ""SomeController"", ""Spring MVC Controller"")
  Component(SoftwareSystem__WebApplication__SomeRepository__6d9009, ""SomeRepository"", ""Spring Data"")
}
Interact2(""1"", User__387cc75, SoftwareSystem__WebApplication__SomeController__341621c, ""Requests /something"", ""HTTP"")
Interact2(""2"", SoftwareSystem__WebApplication__SomeController__341621c, SoftwareSystem__WebApplication__SomeRepository__6d9009, """")
Interact2(""3"", SoftwareSystem__WebApplication__SomeRepository__6d9009, SoftwareSystem__Database__39bccb8, ""select * from something"", ""JDBC"")
@enduml

@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Deployment.puml

' Structurizr.DeploymentView: deployment
title Software System - Deployment

LAYOUT_WITH_LEGEND()

Node(Deployment__Default__DatabaseServer__1edef6c, ""Database Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__DatabaseServer__MySQL__1fa4f18, ""MySQL"", ""MySQL 5.5.x"") {
    ContainerDb(SoftwareSystem__Database1__bb9c73, ""Database"", ""Relational Database Schema"", ""Stores information"")
  }
}
Node(Deployment__Default__WebServer__1e2ffe, ""Web Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__WebServer__ApacheTomcat__2b8afb4, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
    Container(SoftwareSystem__WebApplication1__31f1f25, ""Web Application"", ""Java and spring MVC"", ""Delivers content"")
  }
}
Rel(SoftwareSystem__WebApplication1__31f1f25, SoftwareSystem__Database1__bb9c73, ""Reads from and writes to"", ""JDBC"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeEnterpriseContextView()
        {
            PopulateWorkspace();

            SystemLandscapeView systemLandscapeView = _workspace.Views.SystemLandscapeViews.First();
            _plantUMLWriter.Write(systemLandscapeView, _stringWriter);

            Assert.Equal(
@"@startuml
!include <C4/C4_Context>

' Structurizr.SystemLandscapeView: enterpriseContext
title System Landscape for Some Enterprise

LAYOUT_WITH_LEGEND()

System_Ext(EmailSystem__1934cbe, ""E-mail System"")
Enterprise_Boundary(SomeEnterprise, ""Some Enterprise"") {
  Person(User__3b843b5, ""User"")
  System(SoftwareSystem__7134f, ""Software System"")
}
Rel(EmailSystem__1934cbe, User__3b843b5, ""Delivers e-mails to"")
Rel(SoftwareSystem__7134f, EmailSystem__1934cbe, ""Sends e-mail using"")
Rel(User__3b843b5, SoftwareSystem__7134f, ""Uses"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());

        }

        [Fact]
        public void test_writeEnterpriseContextView_WithCustomBaseUrl()
        {
            PopulateWorkspace();
            SystemLandscapeView systemLandscapeView = _workspace.Views.SystemLandscapeViews.First();

            _plantUMLWriter.CustomBaseUrl = @"https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/";
            _plantUMLWriter.Write(systemLandscapeView, _stringWriter);

            Assert.Equal(
                @"@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Context.puml

' Structurizr.SystemLandscapeView: enterpriseContext
title System Landscape for Some Enterprise

LAYOUT_WITH_LEGEND()

System_Ext(EmailSystem__1934cbe, ""E-mail System"")
Enterprise_Boundary(SomeEnterprise, ""Some Enterprise"") {
  Person(User__3b843b5, ""User"")
  System(SoftwareSystem__7134f, ""Software System"")
}
Rel(EmailSystem__1934cbe, User__3b843b5, ""Delivers e-mails to"")
Rel(SoftwareSystem__7134f, EmailSystem__1934cbe, ""Sends e-mail using"")
Rel(User__3b843b5, SoftwareSystem__7134f, ""Uses"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());

        }

        [Fact]
        public void test_writeSystemContextView()
        {
            PopulateWorkspace();

            SystemContextView systemContextView = _workspace.Views.SystemContextViews.First();
            _plantUMLWriter.Write(systemContextView, _stringWriter);

            Assert.Equal(
@"@startuml
!include <C4/C4_Context>

' Structurizr.SystemContextView: systemContext
title Software System - System Context

LAYOUT_WITH_LEGEND()

Enterprise_Boundary(SomeEnterprise, ""Some Enterprise"") {
  System_Ext(EmailSystem__1127701, ""E-mail System"")
  System(SoftwareSystem__31d545b, ""Software System"")
  Person(User__387cc75, ""User"")
Rel(EmailSystem__1127701, User__387cc75, ""Delivers e-mails to"")
Rel(SoftwareSystem__31d545b, EmailSystem__1127701, ""Sends e-mail using"")
Rel(User__387cc75, SoftwareSystem__31d545b, ""Uses"")
}
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeContainerView()
        {
            PopulateWorkspace();

            ContainerView containerView = _workspace.Views.ContainerViews.First();
            _plantUMLWriter.Write(containerView, _stringWriter);

            Assert.Equal(
@"@startuml
!include <C4/C4_Container>

' Structurizr.ContainerView: containers
title Software System - Containers

LAYOUT_WITH_LEGEND()

System_Ext(EmailSystem__1934cbe, ""E-mail System"")
Person(User__3b843b5, ""User"")
System_Boundary(SoftwareSystem__7134f, ""Software System"") {
  ContainerDb(SoftwareSystem__Database__270f9f2, ""Database"", ""Relational Database Schema"", ""Stores information"")
  Container(SoftwareSystem__WebApplication__1cc1659, ""Web Application"", ""Java and spring MVC"", ""Delivers content"")
}
Rel(EmailSystem__1934cbe, User__3b843b5, ""Delivers e-mails to"")
Rel(User__3b843b5, SoftwareSystem__WebApplication__1cc1659, """", ""HTTP"")
Rel(SoftwareSystem__WebApplication__1cc1659, SoftwareSystem__Database__270f9f2, ""Reads from and writes to"", ""JDBC"")
Rel(SoftwareSystem__WebApplication__1cc1659, EmailSystem__1934cbe, ""Sends e-mail using"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeComponentsView()
        {
            PopulateWorkspace();
            ComponentView componentView = _workspace.Views.ComponentViews.First();

            _plantUMLWriter.Write(componentView, _stringWriter);

            Assert.Equal(
@"@startuml
!include <C4/C4_Component>

' Structurizr.ComponentView: components
title Software System - Web Application - Components

LAYOUT_WITH_LEGEND()

ContainerDb(SoftwareSystem__Database__270f9f2, ""Database"", ""Relational Database Schema"", ""Stores information"")
System_Ext(EmailSystem__1934cbe, ""E-mail System"")
Person(User__3b843b5, ""User"")
Container_Boundary(SoftwareSystem__WebApplication__1cc1659, ""Web Application"") {
  Component(SoftwareSystem__WebApplication__EmailComponent__3d4333b, ""EmailComponent"", """")
  Component(SoftwareSystem__WebApplication__SomeController__327a713, ""SomeController"", ""Spring MVC Controller"")
  Component(SoftwareSystem__WebApplication__SomeRepository__23f6823, ""SomeRepository"", ""Spring Data"")
}
Rel(EmailSystem__1934cbe, User__3b843b5, ""Delivers e-mails to"")
Rel(SoftwareSystem__WebApplication__EmailComponent__3d4333b, EmailSystem__1934cbe, ""Sends e-mails using"", ""SMTP"")
Rel(SoftwareSystem__WebApplication__SomeController__327a713, SoftwareSystem__WebApplication__EmailComponent__3d4333b, ""Sends e-mail using"")
Rel(SoftwareSystem__WebApplication__SomeController__327a713, SoftwareSystem__WebApplication__SomeRepository__23f6823, ""Uses"")
Rel(SoftwareSystem__WebApplication__SomeRepository__23f6823, SoftwareSystem__Database__270f9f2, ""Reads from and writes to"", ""JDBC"")
Rel(User__3b843b5, SoftwareSystem__WebApplication__SomeController__327a713, ""Uses"", ""HTTP"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeDynamicView()
        {
            PopulateWorkspace();

            DynamicView dynamicView = _workspace.Views.DynamicViews.First();
            _plantUMLWriter.Write(dynamicView, _stringWriter);

            // Dynamic diagrams can be drawn with Components 
            Assert.Equal(
@"@startuml
!include <C4/C4_Component>
' C4_Dynamic.puml is missing, simulate it with following definitions
' Scope: Interactions in an enterprise, software system or container.
' Primary and supporting elements: Depends on the diagram scope - 
'     enterprise - people and software systems related to the enterprise in scope 
'     software system - see system context or container diagrams, 
'     container - see component diagram.
' Intended audience: Technical and non-technical people, inside and outside of the software development team.

' Dynamic diagram introduces (automatically) numbered interactions: 
'     Interact(): used automatic calculated index, 
'     Interact2(): index can be explicit defined,
'     SetIndex(): set the next index, 
'     GetIndex(): get the index and automatically increase index

' Index
' ##################################

!function $inc_($value, $step=1)
  !return $value + $step
!endfunction

!$index=1

!function SetIndex($new_index)
  !$index=$new_index
!endfunction

!function GetIndex($auto_increase=1)
  !$old = $index
  !$index=$inc_($index, $auto_increase)
  !return $old
!endfunction

' Interact
' ##################################
!define Interact2(e_index, e_from, e_to, e_label) Rel(e_from, e_to, ""e_index: e_label"")
!define Interact2(e_index, e_from, e_to, e_label, e_techn) Rel(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Back(e_index, e_from, e_to, e_label) Rel_Back(e_from, e_to, ""e_index: e_label"")
!define Interact2_Back(e_index, e_from, e_to, e_label, e_techn) Rel_Back(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Neighbor(e_index, e_from, e_to, e_label) Rel_Neighbor(e_from, e_to, ""e_index: e_label"")
!define Interact2_Neighbor(e_index, e_from, e_to, e_label, e_techn) Rel_Neighbor(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_Back_Neighbor(e_index, e_from, e_to, e_label) Rel_Back_Neighbor(e_from, e_to, ""e_index: e_label"")
!define Interact2_Back_Neighbor(e_index, e_from, e_to, e_label, e_techn) Rel_Back_Neighbor(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_D(e_index, e_from, e_to, e_label) Rel_D(e_from, e_to, ""e_index: e_label"")
!define Interact2_D(e_index, e_from, e_to, e_label, e_techn) Rel_D(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Down(e_index, e_from, e_to, e_label) Rel_Down(e_from, e_to, ""e_index: e_label"")
!define Interact2_Down(e_index, e_from, e_to, e_label, e_techn) Rel_Down(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_U(e_index, e_from, e_to, e_label) Rel_U(e_from, e_to, ""e_index: e_label"")
!define Interact2_U(e_index, e_from, e_to, e_label, e_techn) Rel_U(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Up(e_index, e_from, e_to, e_label) Rel_Up(e_from, e_to, ""e_index: e_label"")
!define Interact2_Up(e_index, e_from, e_to, e_label, e_techn) Rel_Up(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_L(e_index, e_from, e_to, e_label) Rel_L(e_from, e_to, ""e_index: e_label"")
!define Interact2_L(e_index, e_from, e_to, e_label, e_techn) Rel_L(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Left(e_index, e_from, e_to, e_label) Rel_Left(e_from, e_to, ""e_index: e_label"")
!define Interact2_Left(e_index, e_from, e_to, e_label, e_techn) Rel_Left(e_from, e_to, ""e_index: e_label"", e_techn)

!define Interact2_R(e_index, e_from, e_to, e_label) Rel_R(e_from, e_to, ""e_index: e_label"")
!define Interact2_R(e_index, e_from, e_to, e_label, e_techn) Rel_R(e_from, e_to, ""e_index: e_label"", e_techn)
!define Interact2_Right(e_index, e_from, e_to, e_label) Rel_Right(e_from, e_to, ""e_index: e_label"")
!define Interact2_Right(e_index, e_from, e_to, e_label, e_techn) Rel_Right(e_from, e_to, ""e_index: e_label"", e_techn)

!unquoted function Interact($e_from, $e_to, $e_label) 
  Interact2($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact($e_from, $e_to, $e_label, $e_techn) 
  Interact2($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Back($e_from, $e_to, $e_label) 
  Interact2_Back($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Back($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Back($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Neighbor($e_from, $e_to, $e_label) 
  Interact2_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Neighbor($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_Back_Neighbor($e_from, $e_to, $e_label) 
  Interact2_Back_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Back_Neighbor($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Back_Neighbor($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_D($e_from, $e_to, $e_label) 
  Interact2_D($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_D($e_from, $e_to, $e_label, $e_techn) 
  Interact2_D($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Down($e_from, $e_to, $e_label) 
  Interact2_Down($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Down($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Down($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_U($e_from, $e_to, $e_label) 
  Interact2_U($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_U($e_from, $e_to, $e_label, $e_techn) 
  Interact2_U($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Up($e_from, $e_to, $e_label) 
  Interact2_Up($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Up($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Up($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_L($e_from, $e_to, $e_label) 
  Interact2_L($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_L($e_from, $e_to, $e_label, $e_techn) 
  Interact2_L($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Left($e_from, $e_to, $e_label) 
  Interact2_Left($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Left($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Left($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

!unquoted function Interact_R($e_from, $e_to, $e_label) 
  Interact2_R($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_R($e_from, $e_to, $e_label, $e_techn) 
  Interact2_R($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Right($e_from, $e_to, $e_label) 
  Interact2_Right($index, ""$e_from"", ""$e_to"", ""$e_label"")
  !$index=$inc_($index)
!endfunction
!unquoted function Interact_Right($e_from, $e_to, $e_label, $e_techn) 
  Interact2_Right($index, ""$e_from"", ""$e_to"", ""$e_label"", $e_techn)
  !$index=$inc_($index)
!endfunction

' Structurizr.DynamicView: dynamic
title Web Application - Dynamic

LAYOUT_WITH_LEGEND()

ContainerDb(SoftwareSystem__Database__39bccb8, ""Database"", ""Relational Database Schema"", ""Stores information"")
Person(User__387cc75, ""User"")
Container_Boundary(SoftwareSystem__WebApplication__d2a342, ""Web Application"") {
  Component(SoftwareSystem__WebApplication__SomeController__341621c, ""SomeController"", ""Spring MVC Controller"")
  Component(SoftwareSystem__WebApplication__SomeRepository__6d9009, ""SomeRepository"", ""Spring Data"")
}
Interact2(""1"", User__387cc75, SoftwareSystem__WebApplication__SomeController__341621c, ""Requests /something"", ""HTTP"")
Interact2(""2"", SoftwareSystem__WebApplication__SomeController__341621c, SoftwareSystem__WebApplication__SomeRepository__6d9009, """")
Interact2(""3"", SoftwareSystem__WebApplication__SomeRepository__6d9009, SoftwareSystem__Database__39bccb8, ""select * from something"", ""JDBC"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeDeploymentView()
        {
            PopulateWorkspace();

            DeploymentView deploymentView = _workspace.Views.DeploymentViews.First();
            _plantUMLWriter.Write(deploymentView, _stringWriter);

            Assert.Equal(
@"@startuml
!include <C4/C4_Container>
' C4_Deployment.puml is missing, simulate it with following definitions
' Scope: A single software system.
' Primary elements: Deployment nodes and containers within the software system in scope.
' Intended audience: Technical people inside and outside of the software development team; including software architects, developers and operations/support staff.

' Colors
' ##################################
!define NODE_FONT_COLOR    #444444
!define NODE_BG_COLOR      #FFFFFF

' Styling
' ##################################

skinparam rectangle<<node>> {
  Shadowing false
  StereotypeFontSize 0
  FontColor NODE_FONT_COLOR
  BackgroundColor NODE_BG_COLOR
  BorderColor #444444
}

' Layout
' ##################################

!definelong LAYOUT_WITH_LEGEND
hide stereotype
legend right
|=                    |= Type                |
|<NODE_BG_COLOR>      | deployment node      |
|<CONTAINER_BG_COLOR> | deployment container |
endlegend
!enddefinelong

' Nodes
' ##################################
' PlantUML does not support automatic line breaks of container, if e_techn is very long insert line breaks with 
' ""</size>\n<size:TECHN_FONT_SIZE>""
!define Node(e_alias, e_label, e_techn) rectangle ""==e_label\n<size:TECHN_FONT_SIZE>[e_techn]</size>"" <<node>> as e_alias

' Structurizr.DeploymentView: deployment
title Software System - Deployment

LAYOUT_WITH_LEGEND()

Node(Deployment__Default__DatabaseServer__1edef6c, ""Database Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__DatabaseServer__MySQL__1fa4f18, ""MySQL"", ""MySQL 5.5.x"") {
    ContainerDb(SoftwareSystem__Database1__bb9c73, ""Database"", ""Relational Database Schema"", ""Stores information"")
  }
}
Node(Deployment__Default__WebServer__1e2ffe, ""Web Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__WebServer__ApacheTomcat__2b8afb4, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
    Container(SoftwareSystem__WebApplication1__31f1f25, ""Web Application"", ""Java and spring MVC"", ""Delivers content"")
  }
}
Rel(SoftwareSystem__WebApplication1__31f1f25, SoftwareSystem__Database1__bb9c73, ""Reads from and writes to"", ""JDBC"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
        }

        [Fact]
        public void test_writeDeploymentView_WithCustomBaseUrl()
        {
            PopulateWorkspace();
            DeploymentView deploymentView = _workspace.Views.DeploymentViews.First();

            _plantUMLWriter.CustomBaseUrl = @"https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/";
            _plantUMLWriter.Write(deploymentView, _stringWriter);

            Assert.Equal(
@"@startuml
!includeurl https://raw.githubusercontent.com/kirchsth/C4-PlantUML/master/C4_Deployment.puml

' Structurizr.DeploymentView: deployment
title Software System - Deployment

LAYOUT_WITH_LEGEND()

Node(Deployment__Default__DatabaseServer__1edef6c, ""Database Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__DatabaseServer__MySQL__1fa4f18, ""MySQL"", ""MySQL 5.5.x"") {
    ContainerDb(SoftwareSystem__Database1__bb9c73, ""Database"", ""Relational Database Schema"", ""Stores information"")
  }
}
Node(Deployment__Default__WebServer__1e2ffe, ""Web Server"", ""Ubuntu 12.04 LTS"") {
  Node(Deployment__Default__WebServer__ApacheTomcat__2b8afb4, ""Apache Tomcat"", ""Apache Tomcat 8.x"") {
    Container(SoftwareSystem__WebApplication1__31f1f25, ""Web Application"", ""Java and spring MVC"", ""Delivers content"")
  }
}
Rel(SoftwareSystem__WebApplication1__31f1f25, SoftwareSystem__Database1__bb9c73, ""Reads from and writes to"", ""JDBC"")
@enduml

".UnifyNewLine().UnifyHashValues(), _stringWriter.ToString().UnifyHashValues());
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

            Container webApplication = softwareSystem.AddContainer("Web Application", "Delivers content", "Java and spring MVC");
            Container database = softwareSystem.AddContainer("Database", "Stores information", "Relational Database Schema");
            // Additional mark it as database
            database.SetIsDatabase(true);
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

        public void Dispose()
        {
            _stringWriter?.Dispose();
        }
    }
}