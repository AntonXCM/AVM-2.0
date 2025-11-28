using System;
using System.Collections.Generic;

public class RandomPickFactory<T>
{
    public List<T> Products;

    public RandomPickFactory(List<T> products) => Products = products;
    public T Get() => Products[Random.Shared.Next(Products.Count)];
    public override string ToString() => $"Random Pick Factory. Products: {Products}";

}