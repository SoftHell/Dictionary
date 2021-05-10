using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApp.Helpers
{
    public class CustomFloatingPointBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal) ||
                context.Metadata.ModelType == typeof(float) ||
                context.Metadata.ModelType == typeof(double))
            {
                return new FloatingPointModelBinder( 
                    context.Services.GetRequiredService<ILoggerFactory>(), 
                    context.Metadata.ModelType);
            }

            return null;
        }
    }

    public class FloatingPointModelBinder : IModelBinder
    {
        private readonly ILogger<FloatingPointModelBinder>? _logger;
        private readonly Type _floatType;

        public FloatingPointModelBinder(ILoggerFactory? loggerFactory, Type floatType)
        {
            _logger = loggerFactory?.CreateLogger<FloatingPointModelBinder>();
            _floatType = floatType;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Remove unnecessary commas and spaces
            //value = value.Replace(",", string.Empty).Trim();

            _logger?.LogDebug($"Floating point number: {value}");
            value = value.Trim();

            value = value.Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            value = value.Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if (_floatType == typeof(decimal))
            {
                if (!decimal.TryParse(value, out var resultValue))
                {
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        $"Could not parse decimal {value}.");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(resultValue);
            }
            else if (_floatType == typeof(float))
            {
                if (!float.TryParse(value, out var resultValue))
                {
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        $"Could not parse float {value}.");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(resultValue);
            }
            else if (_floatType == typeof(double))
            {
                if (!double.TryParse(value, out var resultValue))
                {
                    bindingContext.ModelState.TryAddModelError(
                        bindingContext.ModelName,
                        $"Could not parse double {value}.");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(resultValue);
            }

            return Task.CompletedTask;
        }
    }
}