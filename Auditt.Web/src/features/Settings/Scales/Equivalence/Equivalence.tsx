import { useEquivalence } from "./useEquivalence";
export const Equivalence = () => {
     const { equivalences = [] } = useEquivalence() ?? {};

    return (
        <div className="space-y-4">
            <div className="pl-8 space-y-2 text-sm font-bold">
                {equivalences.length === 0 ? (
                    <span className="text-gray-500">No hay equivalencias disponibles.</span>
                ) : (
                    equivalences.map((equivalence) => (
                        <div key={equivalence.id} className="flex flex-col mb-4">
                            <label className="font-semibold">{equivalence.name}</label>
                            <span className="text-red-500">Value: {equivalence.value}</span>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};
