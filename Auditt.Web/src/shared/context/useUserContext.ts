import { create } from "zustand";
import { TokenModel, UserResponseModel } from "../../routes/Login/LoginModel";
import { persist } from "zustand/middleware";
import { ClientModel } from "../../features/Clients/ClientModel";

interface UserContext {
	token: TokenModel | null;
	user: UserResponseModel | null;
	client: ClientModel | null;
	isAuthenticated: boolean;
	setIsAuthenticated: (isAuthenticated: boolean) => void;
	setUser: (user: UserResponseModel | null) => void;
	setToken: (token: TokenModel | null) => void;
	setInstitution: (client: ClientModel | null) => void;
}

const useUserContext = create(
	persist<UserContext>(
		(set) => ({
			user: null,
			token: null,
			client: null,
			isAuthenticated: false,
			setIsAuthenticated: (isAuthenticated: boolean) =>
				set((state) => ({ ...state, isAuthenticated })),
			setToken: (token: TokenModel | null) =>
				set((state) => ({ ...state, token })),
			setInstitution: (client: ClientModel | null) =>
				set((state) => ({ ...state, client })),
			setUser: (user: UserResponseModel | null) =>
				set((state) => ({ ...state, user })),
		}),
		{
			name: "Auth",
		}
	)
);

export default useUserContext;
