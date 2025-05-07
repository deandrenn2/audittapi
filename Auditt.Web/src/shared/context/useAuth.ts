// stores/useAuth.ts
import { create } from "zustand";
import { UserResponseModel } from "../../routes/Login/LoginModel";
const apiUrl = import.meta.env.VITE_API_URL;

type AuthStore = {
	user: UserResponseModel | null;
	loading: boolean;
	checkAuth: () => Promise<void>;
	logout: () => Promise<void>;
};

export const useAuth = create<AuthStore>((set) => ({
	user: null,
	loading: true,

	checkAuth: async () => {
		set({ loading: true });
		try {
			const res = await fetch(`${apiUrl}api/user/me`, {
				credentials: "include",
			});

			if (!res.ok) {
				set({ user: null });
			} else {
				const data = await res.json();
				set({ user: data?.data });
			}
		} catch {
			set({ user: null });
		} finally {
			set({ loading: false });
		}
	},

	logout: async () => {
		set({ user: null });
	},
}));
