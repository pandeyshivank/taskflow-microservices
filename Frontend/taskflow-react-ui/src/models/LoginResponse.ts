export interface LoginResponse {
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    token: string;
    refreshToken: string;
    expiresAt: string;
}