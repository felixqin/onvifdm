﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using odm.infra;
namespace odm.ui.activities {
	using global::onvif.services;
	using global::System.Xml.Schema;
	
	public partial class AnalyticsMetadataSettingsView{
		
		#region Model definition
		
		public class Model{
			
			public Model(
				string uri, VideoResolution size, string engineConfToken
			){
				
				this.uri = uri;
				this.size = size;
				this.engineConfToken = engineConfToken;
			}
			private Model(){
			}
			

			public static Model Create(
				string uri,
				VideoResolution size,
				string engineConfToken
			){
				var _this = new Model();
				
				_this.uri = uri;
				_this.size = size;
				_this.engineConfToken = engineConfToken;
				return _this;
			}
		
			public string uri{get;private set;}
			public VideoResolution size{get;private set;}
			public string engineConfToken{get;private set;}
		}
			
		#endregion
	
		#region Result definition
		public abstract class Result{
			private Result() { }
			
			public abstract T Handle<T>(
				
				Func<Model,T> apply,
				Func<T> none
			);
	
			public bool IsApply(){
				return AsApply() != null;
			}
			public virtual Apply AsApply(){ return null; }
			public class Apply : Result {
				public Apply(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Apply AsApply(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> apply,
					Func<T> none
				){
					return apply(
						model
					);
				}
	
			}
			
			public bool IsNone(){
				return AsNone() != null;
			}
			public virtual None AsNone(){ return null; }
			public class None : Result {
				public None(){
					
				}
				
				public override None AsNone(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> apply,
					Func<T> none
				){
					return none(
						
					);
				}
	
			}
			
		}
		#endregion

		public ICommand ApplyCommand{ get; private set; }
		public ICommand NoneCommand{ get; private set; }
		
		IActivityContext<Result> activityContext = null;
		SingleAssignmentDisposable activityCancellationSubscription = new SingleAssignmentDisposable();
		bool activityCompleted = false;
		//activity has been completed
		event Action OnCompleted = null;
		//activity has been failed
		event Action<Exception> OnError = null;
		//activity has been completed successfully
		event Action<Result> OnSuccess = null;
		//activity has been canceled
		event Action OnCancelled = null;
		
		public AnalyticsMetadataSettingsView(Model model, IActivityContext<Result> activityContext) {
			this.activityContext = activityContext;
			if(activityContext!=null){
				activityCancellationSubscription.Disposable = 
					activityContext.RegisterCancellationCallback(() => {
						EnsureAccess(() => {
							CompleteWith(() => {
								if(OnCancelled!=null){
									OnCancelled();
								}
							});
						});
					});
			}
			Init(model);
		}

		
		public void EnsureAccess(Action action){
			if(!CheckAccess()){
				Dispatcher.Invoke(action);
			}else{
				action();
			}
		}

		public void CompleteWith(Action cont){
			if(!activityCompleted){
				activityCompleted = true;
				cont();
				if(OnCompleted!=null){
					OnCompleted();
				}
				activityCancellationSubscription.Dispose();
			}
		}
		public void Success(Result result) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Exception error) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel() {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		public void Success(Func<Result> resultFactory) {
			CompleteWith(() => {
				var result = resultFactory();
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Func<Exception> errorFactory) {
			CompleteWith(() => {
				var error = errorFactory();
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel(Action action) {
			CompleteWith(() => {
				action();
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		
	}
}
