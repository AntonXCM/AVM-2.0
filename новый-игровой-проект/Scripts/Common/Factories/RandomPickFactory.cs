using System;
using System.Collections.Generic;

public class RandomPickFactory<T>
{
    public List<T> Products;

    public RandomPickFactory(List<T> products)
    {
        Products = products;
    }

    public T Get()
    {
        return Products[Random.Shared.Next(Products.Count)];
    }
}