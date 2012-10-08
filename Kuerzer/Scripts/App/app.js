/*global ko, crossroads */
(function () {
    'use strict';
    var ENTER_KEY = 13;
    var baseUri = '@ViewBag.ApiUrl';

    ko.bindingHandlers.clickEditor = {
        init: function (element, valueAccessor) {
            var $element = $(element).hide();
            var $text = $element.prev();
            var $buttons = $("<div class='editConfirm'> \
							<button class='saveEdit' type='button'>Save</button> \
							<button class='cancelEdit' type='button'>Cancel</button> \
						</div>").hide().insertAfter($element);
            var $editElements = $buttons.add($element);

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $buttons.remove();
            });

            var _toggle = function (edit) {
                $text.css({ color: !edit ? 'inherit' : '#aaa' });
                $editElements[edit ? 'show' : 'hide']();
            };

            $text.click(function (e) {
                _toggle(true);
            });

            $editElements.find('.saveEdit').click(function () {
                _toggle(false);
                valueAccessor()($element.val());
            });

            $editElements.find('.cancelEdit').click(function () {
                _toggle(false);
                $(element).val(ko.utils.unwrapObservable(valueAccessor()));
            });
        }
			, update: function (element, valueAccessor) {
			    $(element).val(ko.utils.unwrapObservable(valueAccessor()));
			}
    };



    ko.bindingHandlers.clickToEdit = {
        init: function (element, valueAccessor, allBindingsAccessor, data) {
            var observable = valueAccessor(),

			link = document.createElement('a'),
			editButton = document.createElement('a'),
			input = document.createElement('input');

            element.appendChild(link);
            element.appendChild(editButton);
            element.appendChild(input);
            observable.editing = ko.observable(false);

            ko.applyBindingsToNode(link, {

                text: ($.trim(observable.Title) == '') ? "Title of link" : observable.Title,
                hidden: observable.editing,
                //click: function () { observable.editing(true); },
                attr: { href: observable.LongUrl }
            });

            ko.applyBindingsToNode(editButton, {
                hidden: observable.editing,
                css: { 'edit': {} },
                click: function () { observable.editing(true); },
                text: "Edit"
            });

            ko.applyBindingsToNode(input, {
                value: observable.Title,
                visible: observable.editing,
                css: { 'inputTitle': { width: $(element).width() } },
                hasfocus: observable.editing,
                valueUpdate: 'afterkeydown'
            });
        },

        update: function (element, valueAccessor, allBindingsAccessor, data) {
            // First get the latest data that we're bound to
            var value = valueAccessor(), allBindings = allBindingsAccessor();
            //var bin = ko.utils.unwrapObservable(allBindings['hasfocus']);

            var editing = ko.utils.unwrapObservable(value.editing);

            // Grab some more data from another binding property
            //var hid = allBindings.hidden; // 400ms is default duration unless otherwise specified
            if (editing) {
                $(element).children(":first-child").hide();
                $(element).children(".edit").hide();

                $(element).children(".inputTitle").blur(function () {
                    data.UpdateLink(value);
                });

            } else {


                $(element).children(":first-child").show();
                $(element).children(".edit").show();
                $(element).children(".inputTitle").width($(element).children(":first-child").width());
            }
        }
    };



    ko.bindingHandlers.clickToEditGroup = {
        init: function (element, valueAccessor) {
            var observable = valueAccessor(),

			link = document.createElement('a'),
			input = document.createElement('select');

            element.appendChild(link);
            element.appendChild(input);

            observable.editing = ko.observable(false);

            ko.applyBindingsToNode(link, {

                text: ($.trim(observable.Grouop) == '') ? "GroupName" : observable.Group,
                hidden: observable.editing,
                click: function () { observable.editing(true); },
                attr: { href: observable.LongUrl }
            });

            ko.applyBindingsToNode(input, {
                value: observable.Group,
                visible: observable.editing,
                css: { 'inputGroup': { width: $(element).width() } },
                hasfocus: observable.editing//,
            });
        },

        update: function (element, valueAccessor, allBindingsAccessor) {
            // First get the latest data that we're bound to
            var value = valueAccessor(), allBindings = allBindingsAccessor();

            var editing = ko.utils.unwrapObservable(value.editing);
            // Grab some more data from another binding property
            //var hid = allBindings.hidden; // 400ms is default duration unless otherwise specified
            if (editing) {
                $(element).children(".GroupName").hide();
            } else {
                $(element).children(".GroupName").show();
                $(element).children(".inputGroup").width($(element).children(":first-child").width());
            }
            //$(element).Link.link.show(5000);//css({ display: hid == true ? 'block' : 'none' }); // Make the element visible
            //else
            //$(element).input.css({ display: hid == true ? 'block' : 'none' }); // Make the element visible
        }
    };


    ///////////////////////////////////////////////////////////////////////////////////////////////////
    // represent a single link item
    var Link = function (linkId, longUrl, shortUrl, description, title, group) {

        var self = this;
        self.LinkId = ko.observable(linkId);
        self.LongUrl = ko.observable(longUrl);
        self.ShortUrl = ko.observable(shortUrl);
        self.Description = ko.observable(description);
        self.Title = ko.observable(title);
        self.Group = ko.observable(group);

        self.DeleteLink = function (link) {
            $.ajax({
                url: "api/Links/" + ko.toJS(link).LinkId,
                type: 'DELETE',
                contentType: 'application/json',
                success: function () {
                    viewModel.Links.remove(link);
                },
                error: function (e) {
                    alert('Delete Error: ' + e);
                }
                //			$.ajax({ type: "DELETE", url: 'api/Default1/' + ko.toJS(link).LinkId })
                //                  .done(function () {
                //                  	self.Links.remove(link);
                //                 
            });
        };


        self.UpdateLink = function (data) {
            $.ajax({
                url: "api/Links/" + ko.toJS(data).LinkId,
                type: 'put',
                data: ko.toJSON(data),
                contentType: 'application/json',
                success: function (result) {
                    alert(result);
                },
                error: function (e) {
                    alert('Update Error: ' + e);
                }
            });
        };

    };

    // our main view model
    var ViewModel = function () {
        
        var self = this;
        self.Links = ko.observableArray();
        self.appId = "4fb4af4b-c449-496e-9c46-f2bcf97142d6";
        self.getSecKey = 

       self.AddLink = function(longurl) {

            $.ajax({
                url: "api/Links/" + longurl,
                type: 'put',

                contentType: 'application/json',
                success: function (result) {
                    alert(result);
                },
                error: function (e) {
                    alert('Update Error: ' + e);
                }
            });
        };


        //		self.Links = ko.observableArray(ko.utils.arrayMap(links, function (link) {

        //			return new Link(link.linkId, link.LongUrl, link.ShortUrl, link.Description, link.Title, link.Group);
        //		}));



        //$.getJSON("api/Default1", self.Links);


        $.ajax("api/Links/" + this.appId + this.secKey ,
            {
            type: "get",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                ko.utils.arrayMap(result, function (item) {
                    self.Links.push(new Link(item.LinkId, item.LongUrl, item.ShortUrl, item.Description, item.Title, item.Group));
                });
            }
        });
    };

    var links = ko.utils.parseJson(localStorage.getItem('links-knockout'));


    var viewModel = new ViewModel(links || []);
    ko.applyBindings(viewModel);


    //////	// a custom binding to handle the enter key (could go in a separate library)
    //////	ko.bindingHandlers.enterKey = {
    //////		init: function (element, valueAccessor, allBindingsAccessor, data) {

    //////			var wrappedHandler, newValueAccessor;
    //////			// wrap the handler with a check for the enter key
    //////			wrappedHandler = function (data, event) {

    //////				if (event.keyCode === ENTER_KEY) {

    //////					valueAccessor().call(this, data, event);
    //////				}
    //////			};

    //////			// create a valueAccessor with the options that we would want to pass to the event binding
    //////			newValueAccessor = function () {
    //////				return {
    //////					keyup: wrappedHandler
    //////				};
    //////			};

    //////			// call the real event binding's init function
    //////			ko.bindingHandlers.event.init(element, newValueAccessor, allBindingsAccessor, data);
    //////		}
    //////	};

    //////	// wrapper to hasfocus that also selects text and applies focus async
    //////	ko.bindingHandlers.selectAndFocus = {

    //////		init: function (element, valueAccessor, allBindingsAccessor) {

    //////			ko.bindingHandlers.hasfocus.init(element, valueAccessor, allBindingsAccessor);
    //////			ko.utils.registerEventHandler(element, 'focus', function () {
    //////				element.focus();
    //////			});
    //////		},
    //////		update: function (element, valueAccessor) {
    //////			
    //////			ko.utils.unwrapObservable(valueAccessor()); // for dependency
    //////			// ensure that element is visible before trying to focus
    //////			setTimeout(function () {
    //////				ko.bindingHandlers.hasfocus.update(element, valueAccessor);
    //////			}, 0);
    //////		}
    //////	};
    //////	///////////////////////////////////////////////////////////////////////////////////////////////////
    //////	// represent a single link item
    //////	var Link = function (longUrl, shortUrl, description, title, group, completed) {
    //////		this.LongUrl = ko.observable(longUrl),
    //////			this.ShortUrl = ko.observable(shortUrl),
    //////			this.Description = ko.observable(description),
    //////			this.Title = ko.observable(title),
    //////			this.Group = ko.observable(group);
    //////		this.Completed = ko.observable(completed);
    //////		this.Editing = ko.observable(false);
    //////	};

    //////	// our main view model
    //////	var ViewModel = function (links) {

    //////		init: $.ajax("api/Default1", {
    //////			type: "get",
    //////			contentType: 'application/json; charset=utf-8',
    //////			success: function (result) {
    //////				ko.utils.arrayMap(result, function (item) {
    //////					self.Links.push(new Link(item.LinkId, item.LongUrl, item.ShortUrl, item.Description, item.Title, item.Group));
    //////				});
    //////			}
    //////		});

    //////		var self = this;

    //////		// map array of passed in todos to an observableArray of Link objects
    //////		self.Links = ko.observableArray(ko.utils.arrayMap(links, function (link) {

    //////			return new Link(link.LongUrl, link.ShortUrl, link.Description, link.Title, link.Group, link.Editing, link.Completed);
    //////		}));

    //////		// store the new link value being entered
    //////		self.current = ko.observable();

    //////		self.showMode = ko.observable('all');

    //////		self.filteredLinks = ko.computed(function () {
    //////			switch (self.showMode()) {
    //////				case 'active':
    //////					return self.links().filter(function (link) {
    //////						return !Link.completed();
    //////					});
    //////				case 'completed':
    //////					return self.links().filter(function (link) {
    //////						return Link.completed();
    //////					});
    //////				default:
    //////					return self.Links();
    //////			}
    //////		});

    //////		// add a new link, when enter key is pressed
    //////		self.add = function () {
    //////			alert('add function');
    //////			var current = self.current().trim();
    //////			if (current) {
    //////				var createdlink = new Link(current);

    //////				$.ajax({
    //////					url: "api/Default1",
    //////					type: 'post',
    //////					data: ko.toJSON(createdlink),
    //////					contentType: 'application/json',
    //////					success: function (result) {
    //////						alert(result);
    //////					}
    //////				});

    //////				self.links.push(createdlink);
    //////				self.current('');
    //////			}

    //////		};

    //////		// remove a single link
    //////		self.remove = function (link) {
    //////			self.links.remove(link);
    //////		};

    //////		// remove all completed todos
    //////		self.removeCompleted = function () {
    //////			self.links.remove(function (link) {
    //////				return link.completed();
    //////			});
    //////		};

    //////		// edit an item
    //////		self.editItem = function (item) {
    //////			item.Editing(true);
    //////		};

    //////		// stop editing an item.  Remove the item, if it is now empty
    //////		self.stopEditing = function (item) {
    //////			item.Editing(false);

    //////			if (!item.title().trim()) {
    //////				self.emove(item);
    //////			}
    //////		};

    //		// count of all completed links
    //		self.completedCount = ko.computed(function () {
    //			return ko.utils.arrayFilter(self.Links(), function (link) {
    //				return link.completed();
    //			}).length;
    //		});

    //		// count of links that are not complete
    //		self.remainingCount = ko.computed(function () {
    //			return self.Links().length - self.completedCount();
    //		});

    //		// writeable computed observable to handle marking all complete/incomplete
    //		self.allCompleted = ko.computed({
    //			//always return true/false based on the done flag of all todos
    //			read: function () {
    //				return !self.remainingCount();
    //			},
    //			// set all todos to the written value (true/false)
    //			write: function (newValue) {
    //				ko.utils.arrayForEach(self.Links(), function (link) {
    //					// set even if value is the same, as subscribers are not notified in that case
    //					link.completed(newValue);
    //				});
    //			}
    //		});

    // helper function to keep expressions out of markup
    //////		self.getLabel = function (count) {
    //////			return ko.utils.unwrapObservable(count) === 1 ? 'item' : 'items';
    //////		};


    //////		self.UpdateLinks = function (data) {
    //////			$.ajax({
    //////				url: "API/Default1/PostLink/",
    //////				type: 'post',
    //////				data: ko.toJSON(data),
    //////				contentType: 'application/json',
    //////				success: function (result) {
    //////					alert(result);
    //////				}
    //////			});
    //////		};


    //////		// internal computed observable that fires whenever anything changes in our todos
    //////		ko.computed(function () {
    //////			// store a clean copy to local storage, which also creates a dependency on the observableArray and all observables in each item
    //////			localStorage.setItem('links-knockout', ko.toJSON(self, links));
    //////		}).extend({
    //////			throttle: 500
    //////		}); // save at most twice per second
    //////	};

    //////	// check local storage for todos
    //////	//var links = ko.utils.parseJson(localStorage.getItem('links-knockout'));


    //////	// bind a new instance of our view model to the page
    //////	var viewModel = new ViewModel();




    //////	ko.applyBindings(viewModel);

    //////	//setup crossroads
    //////	crossroads.addRoute('all', function () {
    //////		viewModel.showMode('all');
    //////	});

    //////	crossroads.addRoute('active', function () {
    //////		viewModel.showMode('active');
    //////	});

    //////	crossroads.addRoute('completed', function () {
    //////		viewModel.showMode('completed');
    //////	});

    //////	window.onhashchange = function () {
    //////		crossroads.parse(location.hash.replace('#', ''));
    //////	};

    //////	crossroads.parse(location.hash.replace('#', ''));

}());
