import{_ as e,o as t,c as s,e as n}from"./app-e0605d3e.js";const a={},o=n(`<h1 id="themedinputbox-dialog" tabindex="-1"><a class="header-anchor" href="#themedinputbox-dialog" aria-hidden="true">#</a> ThemedInputBox dialog</h1><p>The <code>ThemedInputBox</code> is a dialog that provides a message and a <code>TextBox</code> so that the user can enter text.</p><h2 id="properties" tabindex="-1"><a class="header-anchor" href="#properties" aria-hidden="true">#</a> Properties</h2><p>None.</p><h2 id="methods" tabindex="-1"><a class="header-anchor" href="#methods" aria-hidden="true">#</a> Methods</h2><h3 id="static-string-show-string-message-string-title-string-defaultvalue" tabindex="-1"><a class="header-anchor" href="#static-string-show-string-message-string-title-string-defaultvalue" aria-hidden="true">#</a> static string? Show(string message, string title, string defaultValue = &quot;&quot;)</h3><p>Shows a modal dialog using the current theme. Returns a <code>string?</code>.<br> If the returned value is <code>null</code>, it means that the user selected the <strong>Cancel</strong> button or closed the dialog.</p><h4 id="arguments" tabindex="-1"><a class="header-anchor" href="#arguments" aria-hidden="true">#</a> Arguments</h4><ul><li><code>string</code> message : The message to display.</li><li><code>string</code> title : The title of the dialog.</li><li><code>string</code> defaultValue : The text to write in the <code>TextBox</code> by default when the dialog opens.</li></ul><h3 id="static-string-show-string-message-string-title-string-defaultvalue-string-defaultbuttontext-string-secondarybuttontext" tabindex="-1"><a class="header-anchor" href="#static-string-show-string-message-string-title-string-defaultvalue-string-defaultbuttontext-string-secondarybuttontext" aria-hidden="true">#</a> static string? Show(string message, string title, string defaultValue, string defaultButtonText, string secondaryButtonText)</h3><p>Shows a modal dialog using the current theme. Returns a <code>string?</code>.<br> If the returned value is <code>null</code>, it means that the user selected the <strong>Cancel</strong> button or closed the dialog.</p><h4 id="arguments-1" tabindex="-1"><a class="header-anchor" href="#arguments-1" aria-hidden="true">#</a> Arguments</h4><ul><li><code>string</code> message : The message to display.</li><li><code>string</code> title : The title of the dialog.</li><li><code>string</code> defaultValue : The text to write in the <code>TextBox</code> by default when the dialog opens.</li><li><code>string</code> defaultButtonText : The text to display in the default button.</li><li><code>string</code> secondaryButtonText : The text to in the the secondary button.</li></ul><h3 id="static-string-show-string-message-string-title-window-owner-string-defaultvalue-string-defaultbuttontext-null-string-secondarybuttontext-null" tabindex="-1"><a class="header-anchor" href="#static-string-show-string-message-string-title-window-owner-string-defaultvalue-string-defaultbuttontext-null-string-secondarybuttontext-null" aria-hidden="true">#</a> static string? Show(string message, string title, Window owner, string defaultValue = &quot;&quot;, string? defaultButtonText = null, string? secondaryButtonText = null</h3><p>Shows a modal dialog using the current theme. Returns a <code>MessageBoxResult</code> that represents the choice of the user.<br> This method can be used to override the default texts and provide a better UX while still using the default framework components.<br> This method should be used with the <code>MessageBoxButton.YesNoCancel</code> or <code>MessageBoxButton.YesNo</code> arguments. The <strong>Yes</strong> button will be replaced with the <strong>defaultButtonText</strong> value, and the <strong>No</strong> button will be replaced with the <strong>secondaryButtonText</strong>.</p><h4 id="arguments-2" tabindex="-1"><a class="header-anchor" href="#arguments-2" aria-hidden="true">#</a> Arguments</h4><ul><li><code>string</code> message : The message to display.</li><li><code>string</code> title : The title of the dialog.</li><li><code>Window</code> owner : The window that owns the dialog; used for the modal behavior.</li><li><code>string</code> defaultValue : The text to write in the <code>TextBox</code> by default when the dialog opens.</li><li><code>string</code> defaultButtonText : The text to display in the default button.</li><li><code>string</code> secondaryButtonText : The text to in the the secondary button.</li></ul><h2 id="events" tabindex="-1"><a class="header-anchor" href="#events" aria-hidden="true">#</a> Events</h2><p>None.</p><h2 id="examples" tabindex="-1"><a class="header-anchor" href="#examples" aria-hidden="true">#</a> Examples</h2><h3 id="example-1" tabindex="-1"><a class="header-anchor" href="#example-1" aria-hidden="true">#</a> Example 1</h3><p>This example shows how to use the <code>ThemedInputBox</code> from C# and handle the result.</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token class-name"><span class="token keyword">string</span><span class="token punctuation">?</span></span> <span class="token keyword">value</span> <span class="token operator">=</span> ThemedInputBox<span class="token punctuation">.</span><span class="token function">Show</span><span class="token punctuation">(</span><span class="token string">&quot;Please provide name for this folder:&quot;</span><span class="token punctuation">,</span> <span class="token string">&quot;Rename folder&quot;</span><span class="token punctuation">,</span> <span class="token keyword">this</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token keyword">if</span> <span class="token punctuation">(</span><span class="token operator">!</span><span class="token keyword">string</span><span class="token punctuation">.</span><span class="token function">IsNullOrEmpty</span><span class="token punctuation">(</span><span class="token keyword">value</span><span class="token punctuation">)</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token comment">// some code to rename a folder</span>
<span class="token punctuation">}</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,23),i=[o];function d(r,l){return t(),s("div",null,i)}const c=e(a,[["render",d],["__file","ThemedInputBox.html.vue"]]);export{c as default};
