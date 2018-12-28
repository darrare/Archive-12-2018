import numpy as np

#%%
# Step 1: Make a random square matrix A of size 3x3
A = np.matrix(np.random.random_sample((3,3)))


#%%
# Step 2: Apply singular value decomposition --
# decompose the matrix into U,S,V such that A=U*S*V

U, S, V = np.linalg.svd(A)

#default function returns  1xN matrix. Converts to diagonal matrix of same value
S = S * np.identity(3) 

#test to make sure A=U*S*V
val = U * S * V
print 'TESTING A VS U*S*V'
print '----------A----------'
print A
print '--------U*S*V--------'
print val


#%%
# Step 3: Replace the matrix of singular values S
# with a diagonal matrix with ones on the diagonal
# (identity matrix).

S = np.identity(3)


#%%
# Step 4: Re-build matrix A using the new singular values.

A = U * S * V
V = np.linalg.transpose(V)


#%%
# Step 5: Verify that the singular values of A are all ones
# by printing them out.

print'Verify singular values of A are all ones'
print np.linalg.svd(A, 1, 0)

#%%
# Step 6: Verify that the absolute value of the determinant
# of A is one.



det = np.linalg.det(A)
print 'Verify that the abs of the determinant of A is one'
print np.absolute(det)

