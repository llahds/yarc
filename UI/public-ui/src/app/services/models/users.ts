export interface Register {
    email: string;
    userName: string;
    password: string;
    confirmPassword: string;
}

export interface AuthenticationToken {
    token: string;
    userName: string;
}