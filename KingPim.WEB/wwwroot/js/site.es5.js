﻿"use strict";

$(document).ready(function () {
    var api_key = "40f2eef1b2884b309ff075ca16508d76";
    $.ajax({
        url: "https://newsapi.org/v1/articles?source=the-next-web&sortBy=latest&apiKey=" + api_key,
        success: function success(result) {
            function truncate(s) {
                var i = s.indexOf(" ", 45);

                var ns = s.slice(0, i + 1);
                ns += " ...";
                return ns;
            }

            $.each(result, /*.articles.slice(0, 3)*/function (key, value) {
                var url = "https://i.ytimg.com/vi/d_T5P-zIIAs/maxresdefault.jpg";

                var article = [' <div class="card col-lg-4" style="border:none;">', '<a target="_blank" href="' + value.url + '">', '<img class="card-img" src="' + value.urlToImage + '" alt="Card image cap">', '<div class="card-body">', '<h4>' + value.title + '</h4>', '<p class="card-text">' + truncate(value.description) + '</p > ', '</div>', '</a>', '</div>'].join("");

                $(".news-feed").append(article);
            });
        }
    });
});

