import Swal from "sweetalert2";
import { useEquivalence } from "./useEquivalence";
import ButtonDeletes from "../../../../shared/components/Buttons/ButtonDeletes";
import ButtonDetails from "../../../../shared/components/Buttons/ButtonDetails";
export const Equivalence = () => {
     const { equivalences, deleteEqvalence } = useEquivalence() ?? {};

    function handleDelete(e: React.MouseEvent<HTMLButtonElement>, id: number): void {
            e.preventDefault();
            Swal.fire({
                title: '¿Estás seguro de eliminar esta Equivalencia?',
                text: 'Esta acción no se puede deshacer',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Confirmar',
                cancelButtonText: 'Cancelar',
                preConfirm: async () => {
                    await deleteEqvalence.mutateAsync(id);
                }
            })
        }

    return (
        <div className="space-y-4">
            <div className="pl-8 space-y-1 text-sm font-bold">
                {equivalences?.map((equivalence) =>(
                     <div key={equivalence.id} className="flex mb-3">
                     <label className="font-semibold mr-2">{equivalence.name}</label>
                     <span className="text-red-500 mr-4">Value: {equivalence.value}</span>
                     <span className=""><ButtonDeletes id={equivalence.id} onDelete={handleDelete}/></span>
                     <span><ButtonDetails url={""}/></span>
                 </div>
                ))}
            </div>
        </div>
    );
};
