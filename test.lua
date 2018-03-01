to_fixed = function(n,m)
    m = m or 2
    return n - n % 1.0/math.pow(10,m)
end
print(to_fixed(1.201,2))
