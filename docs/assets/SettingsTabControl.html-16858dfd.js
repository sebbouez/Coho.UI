import{_ as e,o as t,c as n,e as a}from"./app-47cf4121.js";const i="/Coho.UI/assets/settingstab-f30ee798.png",o={},s=a('<h1 id="settingstabcontrol-control" tabindex="-1"><a class="header-anchor" href="#settingstabcontrol-control" aria-hidden="true">#</a> SettingsTabControl Control</h1><p>Namespace: <code>Coho.UI.Controls.TabControl</code>, inherits from <code>TabControl</code></p><p>The <code>SettingsTabControl</code> provides a TabControl to use in settings purpose.</p><p><img src="'+i+`" alt=""></p><h2 id="properties" tabindex="-1"><a class="header-anchor" href="#properties" aria-hidden="true">#</a> Properties</h2><h3 id="title" tabindex="-1"><a class="header-anchor" href="#title" aria-hidden="true">#</a> Title</h3><p>Type: <code>string</code><br> The main title to be displayed at the top of the control, before the current section title.</p><h2 id="methods" tabindex="-1"><a class="header-anchor" href="#methods" aria-hidden="true">#</a> Methods</h2><p>None.</p><h2 id="events" tabindex="-1"><a class="header-anchor" href="#events" aria-hidden="true">#</a> Events</h2><p>None.</p><h2 id="examples" tabindex="-1"><a class="header-anchor" href="#examples" aria-hidden="true">#</a> Examples</h2><h3 id="example-1" tabindex="-1"><a class="header-anchor" href="#example-1" aria-hidden="true">#</a> Example 1</h3><div class="language-xaml line-numbers-mode" data-ext="xaml"><pre class="language-xaml"><code>&lt;tabControl:SettingsTabControl Title=&quot;{x:Static localization:Resources.MenuSettings}&quot;&gt;
    &lt;tabControl:SettingsTabControlItem Title=&quot;{x:Static localization:Resources.SettingsSectionGeneral}&quot;&gt;
    
    ...
    
    &lt;/tabControl:SettingsTabControlItem&gt;
    &lt;tabControl:SettingsTabControlItem Title=&quot;{x:Static localization:Resources.SettingsSectionEditor}&quot;&gt;
    
    ...
    
    &lt;/tabControl:SettingsTabControlItem&gt;
&lt;/tabControl:SettingsTabControl&gt;
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,14),r=[s];function l(d,c){return t(),n("div",null,r)}const b=e(o,[["render",l],["__file","SettingsTabControl.html.vue"]]);export{b as default};
