import{_ as e,o as n,c as a,e as t}from"./app-47cf4121.js";const s="/Coho.UI/assets/announcer-6d14b832.png",o={},i=t('<h1 id="announcer-control" tabindex="-1"><a class="header-anchor" href="#announcer-control" aria-hidden="true">#</a> Announcer control</h1><p>Namespace: <code>Coho.UI.Controls.Announcer</code></p><p>The <code>Announcer</code> is a slider that allows you to specify the number of items per page and the template of bound items.</p><p><img src="'+s+`" alt=""></p><h2 id="properties" tabindex="-1"><a class="header-anchor" href="#properties" aria-hidden="true">#</a> Properties</h2><h3 id="announcetemplate" tabindex="-1"><a class="header-anchor" href="#announcetemplate" aria-hidden="true">#</a> AnnounceTemplate</h3><p>Type: <code>DataTemplate</code><br> The <strong>AnnounceTemplate</strong> property defines the template to be used to display items in the slider.</p><h3 id="isloadingcontent" tabindex="-1"><a class="header-anchor" href="#isloadingcontent" aria-hidden="true">#</a> IsLoadingContent</h3><p>Type: <code>DependencyProperty, bool</code><br> The <strong>IsLoadingContent</strong> property is used to hide control content and display a <code>LoadingRing</code> to indicate that content is being loaded.</p><h3 id="itemsperview" tabindex="-1"><a class="header-anchor" href="#itemsperview" aria-hidden="true">#</a> ItemsPerView</h3><p>Type: <code>DependencyProperty, int</code><br> The <strong>ItemsPerView</strong> property is used to set the number of items per page. When there are more items than this value, the user can slide content using the arrows on the top right of the control.</p><h3 id="itemssource" tabindex="-1"><a class="header-anchor" href="#itemssource" aria-hidden="true">#</a> ItemsSource</h3><p>Type: <code>IEnumerable&lt;object&gt;</code><br> The <strong>ItemsSource</strong> property is used to set the collection of objects to display.</p><h3 id="label" tabindex="-1"><a class="header-anchor" href="#label" aria-hidden="true">#</a> Label</h3><p>Type: <code>DependencyProperty, string</code><br> The <strong>Label</strong> property is used to set the text displayed at the top of the <code>Announcer</code> control.</p><h2 id="methods" tabindex="-1"><a class="header-anchor" href="#methods" aria-hidden="true">#</a> Methods</h2><p>None.</p><h2 id="events" tabindex="-1"><a class="header-anchor" href="#events" aria-hidden="true">#</a> Events</h2><p>None.</p><h2 id="examples" tabindex="-1"><a class="header-anchor" href="#examples" aria-hidden="true">#</a> Examples</h2><h3 id="example-1" tabindex="-1"><a class="header-anchor" href="#example-1" aria-hidden="true">#</a> Example 1</h3><p>This example shows how to set the <code>AnnounceTemplate</code> property from Xaml.</p><div class="language-xaml line-numbers-mode" data-ext="xaml"><pre class="language-xaml"><code>&lt;announcer:Announcer AnnouncesAreaMargin=&quot;0,8,0,0&quot; Margin=&quot;0,8&quot; ItemsPerView=&quot;4&quot;
                                     x:Name=&quot;AnnouncesPresenter&quot; MinHeight=&quot;200&quot;&gt;
                    &lt;announcer:Announcer.AnnounceTemplate&gt;
                        &lt;DataTemplate&gt;
                            &lt;Border x:Name=&quot;BdrContainer&quot; CornerRadius=&quot;6&quot;&gt;
                                &lt;StackPanel&gt;
                                    &lt;TextBlock Text=&quot;{Binding Title}&quot; FontWeight=&quot;SemiBold&quot; FontSize=&quot;14&quot;
                                               TextWrapping=&quot;Wrap&quot; Margin=&quot;0,0,10,0&quot; /&gt;
                                    &lt;TextBlock Text=&quot;{Binding Content}&quot;
                                               TextWrapping=&quot;Wrap&quot; Margin=&quot;0,0,10,0&quot; /&gt;
                                &lt;/StackPanel&gt;
                            &lt;/Border&gt;
                        &lt;/DataTemplate&gt;
                    &lt;/announcer:Announcer.AnnounceTemplate&gt;
&lt;/announcer:Announcer&gt;
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="example-2" tabindex="-1"><a class="header-anchor" href="#example-2" aria-hidden="true">#</a> Example 2</h3><p>This example shows how to load data from code-behind and set the control in loading mode.</p><div class="language-csharp line-numbers-mode" data-ext="cs"><pre class="language-csharp"><code><span class="token comment">// Put the control in Loading mode, it shows a loading ring</span>
AnnouncesPresenter<span class="token punctuation">.</span>IsLoadingContent <span class="token operator">=</span> <span class="token boolean">true</span><span class="token punctuation">;</span>

OnlineContentService<span class="token punctuation">.</span><span class="token function">GetItems</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">.</span><span class="token function">ContinueWith</span><span class="token punctuation">(</span>r <span class="token operator">=&gt;</span>
<span class="token punctuation">{</span>
    <span class="token comment">// Stop the loading mode to show fetched items</span>
    AnnouncesPresenter<span class="token punctuation">.</span>IsLoadingContent <span class="token operator">=</span> <span class="token boolean">false</span><span class="token punctuation">;</span>
    AnnouncesPresenter<span class="token punctuation">.</span>ItemsSource <span class="token operator">=</span> r<span class="token punctuation">.</span>Result<span class="token punctuation">;</span>

<span class="token punctuation">}</span><span class="token punctuation">,</span> TaskScheduler<span class="token punctuation">.</span><span class="token function">FromCurrentSynchronizationContext</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div><h3 id="example-3" tabindex="-1"><a class="header-anchor" href="#example-3" aria-hidden="true">#</a> Example 3</h3><p>This example shows how to bind a localized string to the <code>Label</code> property in Xaml.</p><div class="language-xaml line-numbers-mode" data-ext="xaml"><pre class="language-xaml"><code>&lt;announcer:Announcer AnnouncesAreaMargin=&quot;0,8,0,0&quot; Margin=&quot;0,8&quot; ItemsPerView=&quot;4&quot;
                     x:Name=&quot;AnnouncesPresenter&quot; MinHeight=&quot;200&quot;
                     Label=&quot;{x:Static localization:Resources.OnlineResourcesDescription}&quot;&gt;
&lt;/announcer:Announcer&gt;
</code></pre><div class="line-numbers" aria-hidden="true"><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div><div class="line-number"></div></div></div>`,29),r=[i];function c(d,l){return n(),a("div",null,r)}const p=e(o,[["render",c],["__file","announcer.html.vue"]]);export{p as default};
