import{_ as t,r as o,o as i,c as l,a as n,d as p,w as c,b as s,e as a}from"./app-e0605d3e.js";const u={},d=a('<h1 id="themedopenfiledialog-dialog" tabindex="-1"><a class="header-anchor" href="#themedopenfiledialog-dialog" aria-hidden="true">#</a> ThemedOpenFileDialog dialog</h1><p>The <code>ThemedOpenFileDialog</code> is a dialog that replaces the default <code>OpenFileDialog</code> using the current theme.</p><h2 id="properties" tabindex="-1"><a class="header-anchor" href="#properties" aria-hidden="true">#</a> Properties</h2><p>None.</p><h2 id="methods" tabindex="-1"><a class="header-anchor" href="#methods" aria-hidden="true">#</a> Methods</h2><h3 id="static-string-show-string-title-dictionary-string-string-filetypes-window-owner" tabindex="-1"><a class="header-anchor" href="#static-string-show-string-title-dictionary-string-string-filetypes-window-owner" aria-hidden="true">#</a> static string? Show(string title, Dictionary&lt;string, string&gt; fileTypes, Window owner)</h3><p>Opens a dialog that lets the user browse the computer to open an existing file. Returns a <code>string?</code>.<br> When the result is null, it means that the user has closed the dialog without selecting a file.</p><h4 id="arguments" tabindex="-1"><a class="header-anchor" href="#arguments" aria-hidden="true">#</a> Arguments</h4><ul><li><code>string</code> title : The title of the dialog</li><li><code>Dictionary&lt;string, string&gt;</code> fileTypes : The supported extensions</li><li><code>Window</code> owner: The Window that will own the dialog</li></ul><h3 id="static-string-show-string-title-themedspecialdialogoptions-options-window-owner-string-defaultbuttontext-null-string-secondarybuttontext-null" tabindex="-1"><a class="header-anchor" href="#static-string-show-string-title-themedspecialdialogoptions-options-window-owner-string-defaultbuttontext-null-string-secondarybuttontext-null" aria-hidden="true">#</a> static string? Show(string title, ThemedSpecialDialogOptions options, Window owner, string? defaultButtonText = null, string? secondaryButtonText = null)</h3><p>Opens a dialog that lets the user browse the computer to open an existing file. Returns a <code>string?</code>.<br> When the result is null, it means that the user has closed the dialog without selecting a file.</p><h4 id="arguments-1" tabindex="-1"><a class="header-anchor" href="#arguments-1" aria-hidden="true">#</a> Arguments</h4>',12),r=n("li",null,[n("code",null,"string"),s(" title : The title of the dialog")],-1),h=n("code",null,"ThemedSpecialDialogOptions",-1),k=n("li",null,[n("code",null,"Window"),s(" owner: The Window that will own the dialog")],-1),m=n("li",null,[n("code",null,"string"),s(" defaultButtonText : The text to display in the default button")],-1),g=n("li",null,[n("code",null,"string"),s(" secondaryButtonText : The text to in the the secondary button")],-1),v=a(`<h2 id="events" tabindex="-1"><a class="header-anchor" href="#events" aria-hidden="true">#</a> Events</h2><p>None.</p><h2 id="examples" tabindex="-1"><a class="header-anchor" href="#examples" aria-hidden="true">#</a> Examples</h2><h3 id="example-1" tabindex="-1"><a class="header-anchor" href="#example-1" aria-hidden="true">#</a> Example 1</h3><p>This example shows how to localize the <code>ThemedOpenFileDialog</code> texts.</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token keyword">public</span> <span class="token function">MainWindow</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>
  <span class="token function">InitializeComponent</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
  Loaded <span class="token operator">+=</span> OnLoaded<span class="token punctuation">;</span>
<span class="token punctuation">}</span>

<span class="token keyword">private</span> <span class="token return-type class-name"><span class="token keyword">void</span></span> <span class="token function">OnLoaded</span><span class="token punctuation">(</span><span class="token class-name"><span class="token keyword">object</span></span> sender<span class="token punctuation">,</span> <span class="token class-name">RoutedEventArgs</span> e<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
  <span class="token comment">// Set Coho.UI Texts resources from your own localization resources</span>
  GenericText<span class="token punctuation">.</span>Cancel <span class="token operator">=</span> Localization<span class="token punctuation">.</span>Resources<span class="token punctuation">.</span>GenericCancel<span class="token punctuation">;</span>
  DialogsText<span class="token punctuation">.</span>Open <span class="token operator">=</span> Localization<span class="token punctuation">.</span>Resources<span class="token punctuation">.</span>GenericOpen<span class="token punctuation">;</span>
  DialogsText<span class="token punctuation">.</span>FileName <span class="token operator">=</span> Localization<span class="token punctuation">.</span>Resources<span class="token punctuation">.</span>GenericFileName<span class="token punctuation">;</span>
  DialogsText<span class="token punctuation">.</span>FileType <span class="token operator">=</span> Localization<span class="token punctuation">.</span>Resources<span class="token punctuation">.</span>GenericFileType<span class="token punctuation">;</span>

  <span class="token range operator">..</span><span class="token punctuation">.</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="example-2" tabindex="-1"><a class="header-anchor" href="#example-2" aria-hidden="true">#</a> Example 2</h3><p>This example shows how to use the <code>ThemedOpenFileDialog</code> from C# and handle the result.</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token class-name">ThemedSpecialDialogOptions</span> options <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token constructor-invocation class-name">ThemedSpecialDialogOptions</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    FileTypes <span class="token operator">=</span>
    <span class="token punctuation">{</span>
        <span class="token punctuation">{</span><span class="token string">&quot;All supported file types (*.md;*.pwdp)&quot;</span><span class="token punctuation">,</span> <span class="token string">&quot;*.md;*.pwdp&quot;</span><span class="token punctuation">}</span><span class="token punctuation">,</span>
        <span class="token punctuation">{</span><span class="token string">&quot;Markdown file (*.md)&quot;</span><span class="token punctuation">,</span> <span class="token string">&quot;*.md&quot;</span><span class="token punctuation">}</span><span class="token punctuation">,</span>
        <span class="token punctuation">{</span><span class="token string">&quot;PowerDocs Project file (*.pwdp)&quot;</span><span class="token punctuation">,</span> <span class="token string">&quot;*.pwdp&quot;</span><span class="token punctuation">}</span>
    <span class="token punctuation">}</span>
<span class="token punctuation">}</span><span class="token punctuation">;</span>

<span class="token class-name"><span class="token keyword">string</span><span class="token punctuation">?</span></span> filePath <span class="token operator">=</span> ThemedOpenFileDialog<span class="token punctuation">.</span><span class="token function">Show</span><span class="token punctuation">(</span>Localization<span class="token punctuation">.</span>Resources<span class="token punctuation">.</span>MenuOpen<span class="token punctuation">,</span> options<span class="token punctuation">,</span> <span class="token keyword">this</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token keyword">if</span> <span class="token punctuation">(</span><span class="token operator">!</span><span class="token keyword">string</span><span class="token punctuation">.</span><span class="token function">IsNullOrEmpty</span><span class="token punctuation">(</span>filePath<span class="token punctuation">)</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// open the selected file in your application</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,9);function w(b,f){const e=o("RouterLink");return i(),l("div",null,[d,n("ul",null,[r,n("li",null,[p(e,{to:"/classes/ThemedSpecialDialogOptions.html"},{default:c(()=>[h]),_:1}),s(" options : The options to configure the dialog")]),k,m,g]),v])}const T=t(u,[["render",w],["__file","ThemedOpenFileDialog.html.vue"]]);export{T as default};
