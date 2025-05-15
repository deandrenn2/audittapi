import { create } from "zustand";
import { persist } from "zustand/middleware";

interface AssessmentContext {
	selectedDataCut: number;
	selectedGuide: number;
	setSelectedDataCut: (selectedDataCut: number) => void;
	setSelectedGuide: (selectedGuide: number) => void;
}

const useAssessmentContext = create(
	persist<AssessmentContext>(
		(set) => ({
			selectedDataCut: 0,
			selectedGuide: 0,
			setSelectedDataCut: (selectedDataCut: number) =>
				set((state) => ({ ...state, selectedDataCut })),
			setSelectedGuide: (selectedGuide: number) =>
				set((state) => ({ ...state, selectedGuide })),
		}),
		{
			name: "AssessmentCurrent",
		}
	)
);

export default useAssessmentContext;
