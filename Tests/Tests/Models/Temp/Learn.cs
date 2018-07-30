// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var Learn = Learn.FromJson(jsonString);

namespace Tests.Models.Temp
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Learn
    {
        public Angularjs angularjs { get; set; }

        public Angulardart angulardart { get; set; }

        public Ariatemplates ariatemplates { get; set; }

        public Atmajs atmajs { get; set; }

        public Aurelia aurelia { get; set; }

        public Backbonejs backbonejs { get; set; }

        public Ampersand ampersand { get; set; }

        public Elm elm { get; set; }

        public Canjs canjs { get; set; }

        public Chaplin chaplin { get; set; }

        public Closure closure { get; set; }

        public Cujo cujo { get; set; }

        public Dart dart { get; set; }

        public Deftjs deftjs { get; set; }

        public Dijon dijon { get; set; }

        public Dojo dojo { get; set; }

        public Duel duel { get; set; }

        public Durandal durandal { get; set; }

        public Emberjs emberjs { get; set; }

        public Enyo enyo { get; set; }

        public Exoskeleton exoskeleton { get; set; }

        public Firebase firebase { get; set; }

        public Flight flight { get; set; }

        public Gwt gwt { get; set; }

        public Humble humble { get; set; }

        public Javascript javascript { get; set; }

        public Es6 es6 { get; set; }

        public JsOfOcaml js_of_ocaml { get; set; }

        public Jquery jquery { get; set; }

        public Kendo kendo { get; set; }

        public Knockback knockback { get; set; }

        public Knockoutjs knockoutjs { get; set; }

        public Lavaca lavaca { get; set; }

        public Marionettejs marionettejs { get; set; }

        public Meteor meteor { get; set; }

        public Mithril mithril { get; set; }

        public Olives olives { get; set; }

        public Polymer polymer { get; set; }

        public Puremvc puremvc { get; set; }

        public Ractive ractive { get; set; }

        public Rappidjs rappidjs { get; set; }

        public React react { get; set; }

        public Reagent reagent { get; set; }

        public Riotjs riotjs { get; set; }

        public Sammyjs sammyjs { get; set; }

        public Sapui5 sapui5 { get; set; }

        [JsonProperty("__invalid_name__scalajs-react")]
        public ScalajsReact __invalid_name__scalajs_react
        {
            get;
            set;
        }

        [JsonProperty("__invalid_name__binding-scala")]
        public BindingScala __invalid_name__binding_scala
        {
            get;
            set;
        }

        public Serenadejs serenadejs { get; set; }

        public Socketstream socketstream { get; set; }

        public Somajs somajs { get; set; }

        public Spine spine { get; set; }

        public Troopjs troopjs { get; set; }

        public Typescript typescript { get; set; }

        public Vue vue { get; set; }

        public Webrx webrx { get; set; }

        public Jsblocks jsblocks { get; set; }

        public Templates templates { get; set; }
    }


    public class Example
    {
        public string name { get; set; }
        public string url { get; set; }
        public string source_url { get; set; }
        public string type { get; set; }
    }

    public class Link
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup
    {
        public string heading { get; set; }
        public List<Link> links { get; set; }
    }

    public class Angularjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example> examples { get; set; }
        public List<LinkGroup> link_groups { get; set; }
    }

    public class Example2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link2
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup2
    {
        public string heading { get; set; }
        public List<Link2> links { get; set; }
    }

    public class Angulardart
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example2> examples { get; set; }
        public List<LinkGroup2> link_groups { get; set; }
    }

    public class Example3
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link3
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup3
    {
        public string heading { get; set; }
        public List<Link3> links { get; set; }
    }

    public class Ariatemplates
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example3> examples { get; set; }
        public List<LinkGroup3> link_groups { get; set; }
    }

    public class Example4
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link4
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup4
    {
        public string heading { get; set; }
        public List<Link4> links { get; set; }
    }

    public class Atmajs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example4> examples { get; set; }
        public List<LinkGroup4> link_groups { get; set; }
    }

    public class Example5
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link5
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup5
    {
        public string heading { get; set; }
        public List<Link5> links { get; set; }
    }

    public class Aurelia
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example5> examples { get; set; }
        public List<LinkGroup5> link_groups { get; set; }
    }

    public class Example6
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link6
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup6
    {
        public string heading { get; set; }
        public List<Link6> links { get; set; }
    }

    public class Backbonejs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example6> examples { get; set; }
        public List<LinkGroup6> link_groups { get; set; }
    }

    public class Example7
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link7
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup7
    {
        public string heading { get; set; }
        public List<Link7> links { get; set; }
    }

    public class Ampersand
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example7> examples { get; set; }
        public List<LinkGroup7> link_groups { get; set; }
    }

    public class Example8
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link8
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup8
    {
        public string heading { get; set; }
        public List<Link8> links { get; set; }
    }

    public class Elm
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example8> examples { get; set; }
        public List<LinkGroup8> link_groups { get; set; }
    }

    public class Example9
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link9
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup9
    {
        public string heading { get; set; }
        public List<Link9> links { get; set; }
    }

    public class Canjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example9> examples { get; set; }
        public List<LinkGroup9> link_groups { get; set; }
    }

    public class Example10
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link10
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup10
    {
        public string heading { get; set; }
        public List<Link10> links { get; set; }
    }

    public class Chaplin
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example10> examples { get; set; }
        public List<LinkGroup10> link_groups { get; set; }
    }

    public class Example11
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link11
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup11
    {
        public string heading { get; set; }
        public List<Link11> links { get; set; }
    }

    public class Closure
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example11> examples { get; set; }
        public List<LinkGroup11> link_groups { get; set; }
    }

    public class Example12
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link12
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup12
    {
        public string heading { get; set; }
        public List<Link12> links { get; set; }
    }

    public class Cujo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example12> examples { get; set; }
        public List<LinkGroup12> link_groups { get; set; }
    }

    public class Example13
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link13
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup13
    {
        public string heading { get; set; }
        public List<Link13> links { get; set; }
    }

    public class Dart
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example13> examples { get; set; }
        public List<LinkGroup13> link_groups { get; set; }
    }

    public class Example14
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link14
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup14
    {
        public string heading { get; set; }
        public List<Link14> links { get; set; }
    }

    public class Deftjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example14> examples { get; set; }
        public List<LinkGroup14> link_groups { get; set; }
    }

    public class Example15
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link15
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup15
    {
        public string heading { get; set; }
        public List<Link15> links { get; set; }
    }

    public class Dijon
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example15> examples { get; set; }
        public List<LinkGroup15> link_groups { get; set; }
    }

    public class Example16
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link16
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup16
    {
        public string heading { get; set; }
        public List<Link16> links { get; set; }
    }

    public class Dojo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example16> examples { get; set; }
        public List<LinkGroup16> link_groups { get; set; }
    }

    public class Example17
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link17
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup17
    {
        public string heading { get; set; }
        public List<Link17> links { get; set; }
    }

    public class Duel
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example17> examples { get; set; }
        public List<LinkGroup17> link_groups { get; set; }
    }

    public class Example18
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link18
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup18
    {
        public string heading { get; set; }
        public List<Link18> links { get; set; }
    }

    public class Durandal
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example18> examples { get; set; }
        public List<LinkGroup18> link_groups { get; set; }
    }

    public class Example19
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link19
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup19
    {
        public string heading { get; set; }
        public List<Link19> links { get; set; }
    }

    public class Emberjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example19> examples { get; set; }
        public List<LinkGroup19> link_groups { get; set; }
    }

    public class Example20
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link20
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup20
    {
        public string heading { get; set; }
        public List<Link20> links { get; set; }
    }

    public class Enyo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example20> examples { get; set; }
        public List<LinkGroup20> link_groups { get; set; }
    }

    public class Example21
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link21
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup21
    {
        public string heading { get; set; }
        public List<Link21> links { get; set; }
    }

    public class Exoskeleton
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example21> examples { get; set; }
        public List<LinkGroup21> link_groups { get; set; }
    }

    public class Example22
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link22
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup22
    {
        public string heading { get; set; }
        public List<Link22> links { get; set; }
    }

    public class Firebase
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example22> examples { get; set; }
        public List<LinkGroup22> link_groups { get; set; }
    }

    public class Example23
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link23
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup23
    {
        public string heading { get; set; }
        public List<Link23> links { get; set; }
    }

    public class Flight
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example23> examples { get; set; }
        public List<LinkGroup23> link_groups { get; set; }
    }

    public class Example24
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link24
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup24
    {
        public string heading { get; set; }
        public List<Link24> links { get; set; }
    }

    public class Gwt
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example24> examples { get; set; }
        public List<LinkGroup24> link_groups { get; set; }
    }

    public class Example25
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link25
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup25
    {
        public string heading { get; set; }
        public List<Link25> links { get; set; }
    }

    public class Humble
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example25> examples { get; set; }
        public List<LinkGroup25> link_groups { get; set; }
    }

    public class Example26
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Javascript
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example26> examples { get; set; }
    }

    public class Example27
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Es6
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example27> examples { get; set; }
    }

    public class Example28
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link26
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup26
    {
        public string heading { get; set; }
        public List<Link26> links { get; set; }
    }

    public class JsOfOcaml
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example28> examples { get; set; }
        public List<LinkGroup26> link_groups { get; set; }
    }

    public class Example29
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link27
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup27
    {
        public string heading { get; set; }
        public List<Link27> links { get; set; }
    }

    public class Jquery
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example29> examples { get; set; }
        public List<LinkGroup27> link_groups { get; set; }
    }

    public class Example30
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link28
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup28
    {
        public string heading { get; set; }
        public List<Link28> links { get; set; }
    }

    public class Kendo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example30> examples { get; set; }
        public List<LinkGroup28> link_groups { get; set; }
    }

    public class Example31
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link29
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup29
    {
        public string heading { get; set; }
        public List<Link29> links { get; set; }
    }

    public class Knockback
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example31> examples { get; set; }
        public List<LinkGroup29> link_groups { get; set; }
    }

    public class Example32
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link30
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup30
    {
        public string heading { get; set; }
        public List<Link30> links { get; set; }
    }

    public class Knockoutjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example32> examples { get; set; }
        public List<LinkGroup30> link_groups { get; set; }
    }

    public class Example33
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link31
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup31
    {
        public string heading { get; set; }
        public List<Link31> links { get; set; }
    }

    public class Lavaca
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example33> examples { get; set; }
        public List<LinkGroup31> link_groups { get; set; }
    }

    public class Example34
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link32
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup32
    {
        public string heading { get; set; }
        public List<Link32> links { get; set; }
    }

    public class Marionettejs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example34> examples { get; set; }
        public List<LinkGroup32> link_groups { get; set; }
    }

    public class Example35
    {
        public string name { get; set; }
        public string url { get; set; }
        public string source_url { get; set; }
    }

    public class Link33
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup33
    {
        public string heading { get; set; }
        public List<Link33> links { get; set; }
    }

    public class Meteor
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example35> examples { get; set; }
        public List<LinkGroup33> link_groups { get; set; }
    }

    public class Example36
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link34
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup34
    {
        public string heading { get; set; }
        public List<Link34> links { get; set; }
    }

    public class Mithril
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example36> examples { get; set; }
        public List<LinkGroup34> link_groups { get; set; }
    }

    public class Example37
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link35
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup35
    {
        public string heading { get; set; }
        public List<Link35> links { get; set; }
    }

    public class Olives
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example37> examples { get; set; }
        public List<LinkGroup35> link_groups { get; set; }
    }

    public class Example38
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link36
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup36
    {
        public string heading { get; set; }
        public List<Link36> links { get; set; }
    }

    public class Polymer
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example38> examples { get; set; }
        public List<LinkGroup36> link_groups { get; set; }
    }

    public class Example39
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link37
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup37
    {
        public string heading { get; set; }
        public List<Link37> links { get; set; }
    }

    public class Puremvc
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example39> examples { get; set; }
        public List<LinkGroup37> link_groups { get; set; }
    }

    public class Example40
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link38
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup38
    {
        public string heading { get; set; }
        public List<Link38> links { get; set; }
    }

    public class Ractive
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example40> examples { get; set; }
        public List<LinkGroup38> link_groups { get; set; }
    }

    public class Example41
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link39
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup39
    {
        public string heading { get; set; }
        public List<Link39> links { get; set; }
    }

    public class Rappidjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example41> examples { get; set; }
        public List<LinkGroup39> link_groups { get; set; }
    }

    public class Example42
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link40
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup40
    {
        public string heading { get; set; }
        public List<Link40> links { get; set; }
    }

    public class React
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example42> examples { get; set; }
        public List<LinkGroup40> link_groups { get; set; }
    }

    public class Example43
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link41
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup41
    {
        public string heading { get; set; }
        public List<Link41> links { get; set; }
    }

    public class Reagent
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example43> examples { get; set; }
        public List<LinkGroup41> link_groups { get; set; }
    }

    public class Example44
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link42
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup42
    {
        public string heading { get; set; }
        public List<Link42> links { get; set; }
    }

    public class Riotjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example44> examples { get; set; }
        public List<LinkGroup42> link_groups { get; set; }
    }

    public class Example45
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link43
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup43
    {
        public string heading { get; set; }
        public List<Link43> links { get; set; }
    }

    public class Sammyjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example45> examples { get; set; }
        public List<LinkGroup43> link_groups { get; set; }
    }

    public class Example46
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link44
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup44
    {
        public string heading { get; set; }
        public List<Link44> links { get; set; }
    }

    public class Sapui5
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example46> examples { get; set; }
        public List<LinkGroup44> link_groups { get; set; }
    }

    public class Example47
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link45
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup45
    {
        public string heading { get; set; }
        public List<Link45> links { get; set; }
    }

    public class ScalajsReact
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example47> examples { get; set; }
        public List<LinkGroup45> link_groups { get; set; }
    }

    public class Example48
    {
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string source_url { get; set; }
    }

    public class Link46
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup46
    {
        public string heading { get; set; }
        public List<Link46> links { get; set; }
    }

    public class BindingScala
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example48> examples { get; set; }
        public List<LinkGroup46> link_groups { get; set; }
    }

    public class Example49
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link47
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup47
    {
        public string heading { get; set; }
        public List<Link47> links { get; set; }
    }

    public class Serenadejs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example49> examples { get; set; }
        public List<LinkGroup47> link_groups { get; set; }
    }

    public class Example50
    {
        public string name { get; set; }
        public string url { get; set; }
        public string source_url { get; set; }
    }

    public class Link48
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup48
    {
        public string heading { get; set; }
        public List<Link48> links { get; set; }
    }

    public class Socketstream
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example50> examples { get; set; }
        public List<LinkGroup48> link_groups { get; set; }
    }

    public class Example51
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link49
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup49
    {
        public string heading { get; set; }
        public List<Link49> links { get; set; }
    }

    public class Somajs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example51> examples { get; set; }
        public List<LinkGroup49> link_groups { get; set; }
    }

    public class Example52
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link50
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup50
    {
        public string heading { get; set; }
        public List<Link50> links { get; set; }
    }

    public class Spine
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example52> examples { get; set; }
        public List<LinkGroup50> link_groups { get; set; }
    }

    public class Example53
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link51
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup51
    {
        public string heading { get; set; }
        public List<Link51> links { get; set; }
    }

    public class Troopjs
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example53> examples { get; set; }
        public List<LinkGroup51> link_groups { get; set; }
    }

    public class Example54
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link52
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup52
    {
        public string heading { get; set; }
        public List<Link52> links { get; set; }
    }

    public class Typescript
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example54> examples { get; set; }
        public List<LinkGroup52> link_groups { get; set; }
    }

    public class Example55
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link53
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup53
    {
        public string heading { get; set; }
        public List<Link53> links { get; set; }
    }

    public class Vue
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example55> examples { get; set; }
        public List<LinkGroup53> link_groups { get; set; }
    }

    public class Example56
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link54
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Example57
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link55
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup55
    {
        public string heading { get; set; }
        public List<Link55> links { get; set; }
    }

    public class Angular2
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example57> examples { get; set; }
        public List<LinkGroup55> link_groups { get; set; }
    }

    public class LinkGroup54
    {
        public string heading { get; set; }
        public List<Link54> links { get; set; }
        public Angular2 angular2 { get; set; }
    }

    public class Webrx
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example56> examples { get; set; }
        public List<LinkGroup54> link_groups { get; set; }
    }

    public class Example58
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Link56
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class LinkGroup56
    {
        public string heading { get; set; }
        public List<Link56> links { get; set; }
    }

    public class Jsblocks
    {
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public List<Example58> examples { get; set; }
        public List<LinkGroup56> link_groups { get; set; }
    }

    public class Templates
    {
        public string todomvc { get; set; }
    }
    }   
