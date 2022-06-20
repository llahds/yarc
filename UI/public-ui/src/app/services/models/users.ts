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

export interface UserSettings {
    displayName: string;
    about: string;
}

export interface ChangeUserName {
    userName: string;
    password: string;
}

export interface ChangeEmail {
    email: string;
    password: string;
}

export interface ChangePassword {
    oldPassword: string;
    password: string;
    confirmPassword: string;
}