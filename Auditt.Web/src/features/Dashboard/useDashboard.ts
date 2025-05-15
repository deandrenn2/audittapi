import { useQuery } from "@tanstack/react-query";
import { getDashboard } from "./DashboardServices";
const key = "Dashboard";
export const useDashboard = () => {
	const queryDashboard = useQuery({
		queryKey: [key],
		queryFn: () => getDashboard(),
	});

	return {
		dashboard: queryDashboard.data?.data,
		queryDashboard,
	};
};
